using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;
using Trill.Modules.Timeline.Core.Data;
using Trill.Shared.Abstractions;

namespace Trill.Modules.Timeline.Core.Persistence
{
    internal class RedisStorage : IStorage
    {
        private readonly IDatabase _database;

        public RedisStorage(IDatabase database)
        {
            _database = database;
        }

        public async Task<Paged<Story>> GetTimelineAsync(Guid userId, DateTime? from = null, DateTime? to = null)
        {
            var now = DateTime.UtcNow;
            var minScore = GetScore(from ?? DateTime.UtcNow.AddDays(-7));
            var maxScore = GetScore(to ?? DateTime.UtcNow);
            var storyIds = await _database.SortedSetRangeByScoreAsync(GetTimelineKey(userId), minScore, maxScore);
            var storyKeys = storyIds.Select(x => (RedisKey) GetStoryKey((long)x)).ToArray();
            var storyEntries = await _database.StringGetAsync(storyKeys);
            var stories = new List<Story>();
            foreach (var entry in storyEntries)
            {
                if (!entry.HasValue)
                {
                    continue;
                }

                var story = JsonSerializer.Deserialize<Story>(entry);
                if (story.Visibility.From <= now && story.Visibility.To >= now)
                {
                    stories.Add(story);
                }
            }

            return new Paged<Story>
            {
                Items = stories,
                CurrentPage = 1,
                TotalPages = 1,
                TotalResults = stories.Count,
                ResultsPerPage = stories.Count
            };
        }

        public async Task<IReadOnlyCollection<Guid>> GetAllFollowersAsync(Guid userId)
        {
            var followers = await _database.SetMembersAsync(GetFollowersKey(userId));

            return followers.Select(x => Guid.Parse(x)).ToArray();
        }

        public async Task FollowAsync(Guid followerId, Guid followeeId)
        {
            await _database.SetAddAsync(GetFollowersKey(followeeId), followerId.ToString("N"));
        }

        public async Task UnfollowAsync(Guid followerId, Guid followeeId)
        {
            await _database.SetRemoveAsync(GetFollowersKey(followeeId), followerId.ToString("N"));
        }

        public async Task AddStoryAsync(Story story)
        {
            await _database.StringSetAsync(GetStoryKey(story.Id), JsonSerializer.Serialize(story));
        }

        public async Task SetStoryTotalRatingAsync(long storyId, int totalRate)
        {
            var entry = await _database.StringGetAsync(GetStoryKey(storyId));
            if (!entry.HasValue)
            {
                return;
            }

            var story = JsonSerializer.Deserialize<Story>(entry);
            story.TotalRate = totalRate;
            await AddStoryAsync(story);
        }

        public async Task AddStoryToTimelineAsync(Guid userId, Story story)
        {
            var score = GetScore(story.Visibility.From);
            var entry = new SortedSetEntry(story.Id.ToString("N"), score);
            await _database.SortedSetAddAsync(GetTimelineKey(userId), new[] {entry});
        }

        private static double GetScore(DateTime dateTime) => dateTime.ToUnixTimeMilliseconds();

        private static string GetFollowersKey(Guid userId) => $"timeline:users:{userId:N}:followers";

        private static string GetTimelineKey(Guid userId) => $"timeline:users:{userId:N}:timeline";

        private static string GetStoryKey(long storyId) => $"timeline:stories:{storyId}";
    }
}