using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Trill.Modules.Ads.Core.Domain;

namespace Trill.Modules.Ads.Core.Persistence
{
    internal sealed class AdRepository : IAdRepository
    {
        private const string Schema = "ads-module";
        private readonly IMongoCollection<Ad> _collection;

        public AdRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Ad>($"{Schema}.ads");
        }

        public Task<Ad> GetAsync(Guid id) => _collection.AsQueryable().SingleOrDefaultAsync(x => x.Id == id);

        public Task AddAsync(Ad ad) => _collection.InsertOneAsync(ad);

        public Task UpdateAsync(Ad ad) => _collection.ReplaceOneAsync(x => x.Id == ad.Id, ad);
    }
}