using MongoDB.Driver;
using WhiskyWine.BottleService.Data.Models;
using WhiskyWine.BottleService.Domain.Interfaces;

namespace WhiskyWine.BottleService.Data
{
    /// <summary>
    /// This class contains the database context for mongodb. It will be injected into the mongo repository via the IMongoDbConext interface.
    /// </summary>
    public class BottleMongoDbContext : IMongoDbContext<BottleMongoModel>
    {
        /// <summary>
        /// The mongodb collection property. The bottle entities in the database will be interacted with through this collection.
        /// </summary>
        public IMongoCollection<BottleMongoModel> Collection { get => _collection; }
        
        /// <summary>
        /// The database settings including connection string, database name, collection name.
        /// </summary>
        private readonly IDatabaseSettings _dbSettings;

        /// <summary>
        /// The mongo database.
        /// </summary>
        private readonly IMongoDatabase _database = null;

        /// <summary>
        /// The backing field for the Collection property.
        /// </summary>
        private readonly IMongoCollection<BottleMongoModel> _collection;

        /// <summary>
        /// Constructs an instance of the BottleMongoDbContext class.
        /// </summary>
        /// <param name="dbSettings">An instance of a class implementing the IDatabaseSettings interface. Will contain the connection string, database name, and collection name.</param>
        public BottleMongoDbContext(IDatabaseSettings dbSettings)
        {
            this._dbSettings = dbSettings;

            //Spin up a new mongo client. Get the database from the client, and the collection from the database.
            var client = new MongoClient(dbSettings.ConnectionString);
            if (client != null) _database = client.GetDatabase(dbSettings.DatabaseName);
            this._collection = _database.GetCollection<BottleMongoModel>(_dbSettings.BottlesCollectionName);
        }
    }
}
