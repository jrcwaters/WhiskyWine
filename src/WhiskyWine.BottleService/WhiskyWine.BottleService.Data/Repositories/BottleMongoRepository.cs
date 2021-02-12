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

        public async Task<Bottle> InsertAsync(Bottle entity)
        {
            await _bottles.InsertOneAsync(entity);
            return entity;
        }

        public async Task<Bottle> GetByIdAsync(string id)
        {
            return (await _bottles.FindAsync(bottle => bottle.BottleId == id)).FirstOrDefault();
        }

        public async Task<IEnumerable<Bottle>> GetAllAsync()
        {
            return (await _bottles.FindAsync(c => true)).ToList();
        }

        public async Task UpdateAsync(string id, Bottle entity)
        {
            await _bottles.ReplaceOneAsync(bottle => bottle.BottleId == id, entity);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return (await _bottles.DeleteOneAsync(c => c.BottleId == id)).IsAcknowledged;
        }
    }
}
