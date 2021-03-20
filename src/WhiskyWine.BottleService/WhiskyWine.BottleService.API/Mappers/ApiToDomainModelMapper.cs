using System;
using System.Collections.Generic;
using WhiskyWine.BottleService.API.Models;
using WhiskyWine.BottleService.Domain.Enums;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.API.Mappers
{
    /// <summary>
    /// The adapter class used to map from BottleApiModel models used to communicate Bottle data in and out of the API to domain Bottle models.
    /// This class forms one of the adapters for the ports and adapter architecture of the service, the corresponding port being the Bottles REST API.
    /// </summary>
    public class ApiToDomainModelMapper : IMapper<BottleApiModel, BottleDomainModel>
    {
        /// <summary>
        /// Maps a Bottle API model to a domain model.
        /// </summary>
        /// <param name="from">API model to map from.</param>
        /// <returns>Domain model that results from the mapping.</returns>
        public BottleDomainModel MapOne(BottleApiModel from)
        {
            if (from == null) return null;

            if (!Enum.TryParse(from.AlcoholCategory, true, out AlcoholCategory alcoholCategory))
            {
                alcoholCategory = AlcoholCategory.Unknown;
            }

            return new BottleDomainModel
            {
                BottleId = from.BottleId,
                Name = from.Name,
                Region = from.Region,
                AlcoholCategory = alcoholCategory
               
            };

        }

        /// <summary>
        /// Maps an IEnumerable of API models to an IEnumerable of domain models.
        /// </summary>
        /// <param name="from">The IEnumerable of API models from which to Map.</param>
        /// <returns>IEnumumerable of domain models, each mapped from an API model in the input.</returns>
        public IEnumerable<BottleDomainModel> MapMany(IEnumerable<BottleApiModel> from)
        {
            var mappedList = new List<BottleDomainModel>();
            foreach (var apiBottle in from)
            {
                mappedList.Add(MapOne(apiBottle));
            }
            return mappedList;
        }
    }
}
