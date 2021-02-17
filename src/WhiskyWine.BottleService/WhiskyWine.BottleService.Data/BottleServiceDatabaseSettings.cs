using WhiskyWine.BottleService.Domain.Interfaces;

namespace WhiskyWine.BottleService.Data
{
    public class BottleServiceDatabaseSettings : IDatabaseSettings
    {
        public string BottlesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
