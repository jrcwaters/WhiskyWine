using MongoDB.Bson;
using WhiskyWine.BottleService.Data.Models;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.Data.Mappers
{
    public class DomainToMongoModelMapper : IMapper<Bottle, BottleMongoModel>
    {
        public BottleMongoModel Map(Bottle fromType)
        {
            if (fromType == null) return null;

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
