using MongoDB.Bson;
using WhiskyWine.BottleService.Data.Models;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.Data.Mappers
{
    /// <summary>
    /// The adapter class used to map from domain Bottle models to BottleMongoModel models used to interact with the mongo database.
    /// This class forms one of the adapters for the ports and adapter architecture of the service.
    /// </summary>
    public class DomainToMongoModelMapper : IMapper<Bottle, BottleMongoModel>
    {
        /// <summary>
        /// Maps a domain Bottle model to a BottleMongoModel so that bottle data can be written to mongo database.
        /// </summary>
        /// <param name="fromType">The Bottle to map from.</param>
        /// <returns>The BottleMongoModel resulting from the mapping.</returns>
        public BottleMongoModel Map(Bottle fromType)
        {
            if (fromType == null) return null;

            //If the Id of the passed Bottle is not parseable to mongo ObjectId set it to empty.
            if (!ObjectId.TryParse(fromType.BottleId, out ObjectId objectId))
            {
                objectId = ObjectId.Empty;
            }

            return new BottleMongoModel
            {
                BottleId = objectId,
                Name = fromType.Name,
                Region = fromType.Region,
                AlcoholCategory = fromType.AlcoholCategory
            };
        }
    }
}
