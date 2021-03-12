using WhiskyWine.BottleService.Domain.Interfaces;

namespace WhiskyWine.BottleService.Data
{
    /// <summary>
    /// This class is used to hold the connection, database, and collection properties for mongodb.
    /// </summary>
    public class BottleServiceDatabaseSettings : IDatabaseSettings
    {
        public string BottlesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
