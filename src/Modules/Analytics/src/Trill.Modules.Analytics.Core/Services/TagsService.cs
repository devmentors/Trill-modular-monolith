using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Trill.Modules.Analytics.Core.Mongo;
using Tag = Trill.Modules.Analytics.Core.Models.Tag;

namespace Trill.Modules.Analytics.Core.Services
{
    internal class TagsService : ITagsService
    {
        private readonly IDatabaseProvider _databaseProvider;

        public TagsService(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public async Task<bool> TryAddAsync(Tag tag)
        {
            try
            {
                if (await _databaseProvider.Tags.AsQueryable().AnyAsync(x => x.Name == tag.Name))
                {
                    return false;
                }

                await _databaseProvider.Tags.InsertOneAsync(tag);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task IncrementOccurrencesCountAsync(string tag)
        {
            var builder = Builders<Tag>.Filter;
            var filter = builder.Eq(x => x.Name, tag);
            var update = Builders<Tag>.Update.Inc(s => s.OccurenceCount, 1);
            await _databaseProvider.Tags.FindOneAndUpdateAsync(filter, update);
        }
    }
}