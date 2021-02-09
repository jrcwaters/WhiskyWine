using Microsoft.Extensions.Caching.Memory;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.Data.Repositories
{
    public class BottleMongoRepository : IRepository<Bottle>
    {

        private readonly IMemoryCache _cache;
        private readonly IMongoDatabase _mongoDatabase;

        public BottleMongoRepository(IMemoryCache cache, IMongoDatabase mongoDatabase)
        {
            this._cache = cache;
            this._mongoDatabase = mongoDatabase;
        }

        public async Task<Bottle> Insert(Bottle entity)
        {
            var bottlesCollection = _mongoDatabase.GetCollection<Bottle>(nameof(Bottle));
            await bottlesCollection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<Bottle> GetById(int id)
        {
            var bottlesCollection = _mongoDatabase.GetCollection<Bottle>(nameof(Bottle));
            return (await bottlesCollection.FindAsync(c => c._id == id)).FirstOrDefault();
        }

        public async Task<IEnumerable<Bottle>> GetAll()
        {
            var bottlesCollection = _mongoDatabase.GetCollection<Bottle>(nameof(Bottle));
            return (await bottlesCollection.FindAsync(new BsonDocument())).ToList();
        }

        public async Task<Bottle> Update(int id, Bottle entity)
        {
            var bottlesCollection = _mongoDatabase.GetCollection<Bottle>(nameof(Bottle));
            var result = await bottlesCollection.ReplaceOneAsync(c => c._id == id, entity);

            if (!result.IsAcknowledged) return null;

            entity.BottleId = id;
            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var bottlesCollection = _mongoDatabase.GetCollection<Bottle>(nameof(Bottle));
            return (await bottlesCollection.DeleteOneAsync(c => c._id == id)).IsAcknowledged;
        }
    }
}
