
namespace WhiskyWine.BottleService.Domain.Interfaces
{
    public interface IDatabaseSettings
    {
        string BottlesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
