using WhiskyWine.BottleService.Data.Models;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.Data.Mappers
{
    public class MongoToDomainModelMapper : IMapper<BottleMongoModel, Bottle>
    {
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
