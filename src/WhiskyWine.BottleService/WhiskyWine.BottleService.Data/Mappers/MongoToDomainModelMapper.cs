using WhiskyWine.BottleService.Data.Models;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.Data.Mappers
{
    /// <summary>
    /// The adapter class used to map from BottleMongoModel models used to interact with the mongo database to domain Bottle models.
    /// This class forms one of the adapters for the ports and adapter architecture of the service.
    /// </summary>
    public class MongoToDomainModelMapper : IMapper<BottleMongoModel, Bottle>
    {
        /// <summary>
        /// Maps a BottleMongoModel to a domain Bottle model so that bottle data from mongo can be passed back up to domain layer.
        /// </summary>
        /// <param name="fromType">The BottleMongoModel to map from.</param>
        /// <returns>The Bottle resulting from the mapping.</returns>
        public Bottle Map(BottleMongoModel fromType)
        {
            if (fromType == null) return null;
            return new Bottle
            {
                BottleId = fromType.BottleId.ToString(),
                Name = fromType.Name,
                Region = fromType.Region,
                AlcoholCategory = fromType.AlcoholCategory
            };
        }
    }
}
