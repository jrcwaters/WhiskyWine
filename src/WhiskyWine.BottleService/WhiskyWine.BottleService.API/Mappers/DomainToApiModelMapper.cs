using System.Collections.Generic;
using WhiskyWine.BottleService.API.Models;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.API.Mappers
{
    public class DomainToApiModelMapper : IMapper<BottleDomainModel, BottleApiModel>
    {
        public BottleApiModel MapOne(BottleDomainModel fromType)
        {
            if (fromType == null) return null;

            return new BottleApiModel
            {
                BottleId = fromType.BottleId,
                Name = fromType.Name,
                Region = fromType.Region,
                AlcoholCategory = fromType.AlcoholCategory.ToString()
            };
        }

        public IEnumerable<BottleApiModel> MapMany(IEnumerable<BottleDomainModel> from)
        {
            var mappedList = new List<BottleApiModel>();
            foreach (var bottle in from)
            {
                mappedList.Add(MapOne(bottle));
            }
            return mappedList;
        }
    }
}
