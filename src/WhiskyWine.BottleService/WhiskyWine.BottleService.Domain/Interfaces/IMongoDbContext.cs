using MongoDB.Driver;

namespace WhiskyWine.BottleService.Domain.Interfaces
{
    /// <summary>
    /// Interface that specifies the contract that must be met by classes acting as database context for mongodb.
    /// Generic type parameter T specifies the type of the object stored in the mongo data store.
    /// </summary>
    public interface IMongoDbContext<T>
    {
        /// <summary>
        /// The collection of objects of type T in the data store.
        /// </summary>
        public IMongoCollection<T> Collection { get; }
    }
}
