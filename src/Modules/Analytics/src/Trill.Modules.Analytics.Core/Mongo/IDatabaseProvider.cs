using MongoDB.Driver;
using Trill.Modules.Analytics.Core.Models;
using Tag = Trill.Modules.Analytics.Core.Models.Tag;

namespace Trill.Modules.Analytics.Core.Mongo
{
    internal interface IDatabaseProvider
    {
        IMongoCollection<Story> Stories { get; }
        IMongoCollection<Tag> Tags { get; }
        IMongoCollection<User> Users { get; }
    }
}