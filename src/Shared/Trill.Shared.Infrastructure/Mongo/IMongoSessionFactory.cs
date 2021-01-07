using System.Threading.Tasks;
using MongoDB.Driver;

namespace Trill.Shared.Infrastructure.Mongo
{
    public interface IMongoSessionFactory
    {
        Task<IClientSessionHandle> CreateAsync();
    }
}