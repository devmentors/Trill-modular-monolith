using System.Threading.Tasks;
using MongoDB.Driver;

namespace Trill.Shared.Infrastructure.Mongo
{
    public interface IMongoDbSeeder
    {
        Task SeedAsync(IMongoDatabase database);
    }
}