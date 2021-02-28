using MongoDB.Driver;
using WhiskyWine.BottleService.Data.Models;
using WhiskyWine.BottleService.Domain.Interfaces;

namespace WhiskyWine.BottleService.Data
{
    public class BottleMongoDbContext : IMongoDbContext<BottleMongoModel>
    {
        public IMongoCollection<BottleMongoModel> Collection { get => _collection; }

        private readonly IDatabaseSettings _dbSettings;
        private readonly IMongoDatabase _database = null;
        private readonly IMongoCollection<BottleMongoModel> _collection;

        public BottleMongoDbContext(IDatabaseSettings dbSettings)
        {
            this._dbSettings = dbSettings;
            var client = new MongoClient(dbSettings.ConnectionString);
            if (client != null) _database = client.GetDatabase(dbSettings.DatabaseName);
            this._collection = _database.GetCollection<BottleMongoModel>(_dbSettings.BottlesCollectionName);
        }
    }
}
