using System;
using MongoDB.Driver;
using Trill.Shared.Infrastructure.Mongo;

namespace Trill.Shared.Tests.Integration
{
    public class MongoFixture : IDisposable
    {
        private readonly MongoClient _client;
        private readonly string _databaseName;

        public MongoFixture()
        {
            var options = OptionsHelper.GetOptions<MongoOptions>("mongo");
            _client = new MongoClient(options.ConnectionString);
            _databaseName = options.Database;
        }

        public IMongoDatabase Database => _client.GetDatabase(_databaseName);

        public void Dispose()
        {
            _client.DropDatabase(_databaseName);
        }
    }
}