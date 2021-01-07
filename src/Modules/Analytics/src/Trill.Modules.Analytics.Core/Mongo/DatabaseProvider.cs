using MongoDB.Driver;
using Trill.Modules.Analytics.Core.Models;
using Tag = Trill.Modules.Analytics.Core.Models.Tag;

namespace Trill.Modules.Analytics.Core.Mongo
{
    internal class DatabaseProvider : IDatabaseProvider
    {
        private const string Schema = "analytics-module";
        private readonly IMongoDatabase _database;

        public DatabaseProvider(IMongoDatabase database)
        {
            _database = database;
        }

        public IMongoCollection<Story> Stories => _database.GetCollection<Story>($"{Schema}.stories");
        public IMongoCollection<Tag> Tags => _database.GetCollection<Tag>($"{Schema}.tags");
        public IMongoCollection<User> Users => _database.GetCollection<User>($"{Schema}.users");
    }
}