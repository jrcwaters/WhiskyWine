
namespace WhiskyWine.BottleService.Domain.Interfaces
{
    /// <summary>
    /// Interface that specifies the contract that must be met by classes used to hold database settings.
    /// </summary>
    public interface IDatabaseSettings
    {
        /// <summary>
        /// The name of the collection of Bottles in the data store.
        /// </summary>
        string BottlesCollectionName { get; set; }

        /// <summary>
        /// The connection string to the database.
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// The name of the database in the data store.
        /// </summary>
        string DatabaseName { get; set; }
    }
}
