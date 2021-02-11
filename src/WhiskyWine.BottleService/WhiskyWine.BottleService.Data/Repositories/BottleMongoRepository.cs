using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.Data.Repositories
{
    public class BottleMongoRepository : IRepository<Bottle>
    {
        private readonly IMongoCollection<Bottle> _bottles;

        public BottleMongoRepository(IBottleServiceDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            
            this._bottles = database.GetCollection<Bottle>(settings.BottlesCollectionName);
        }

        public async Task<Bottle> Insert(Bottle entity)
        {
            await _bottles.InsertOneAsync(entity);
            return entity;
        }

        public async Task<Bottle> GetById(int id)
        {
            return (await _bottles.FindAsync(bottle => bottle.Id == id)).FirstOrDefault();
        }

        public async Task<IEnumerable<Bottle>> GetAll()
        {
            return (await _bottles.FindAsync(c => true)).ToList();
        }

        public async Task<Bottle> Update(int id, Bottle entity)
        {
            var result = await _bottles.ReplaceOneAsync(bottle => bottle.Id == id, entity);

            if (!result.IsAcknowledged) return null;

            entity.Id = id;
            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            return (await _bottles.DeleteOneAsync(c => c.Id == id)).IsAcknowledged;
        }
    }
}
