using System.Collections.Generic;
using WhiskyWine.BottleService.API.Models;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.API.Mappers
{
    /// <summary>
    /// The adapter class used to map from BottleDomainModel models to the BottleApiModels that are used to communicate Bottle data in and out of the REST API.
    /// This class forms one of the adapters for the ports and adapter architecture of the service, the corresponding port being the Bottles REST API.
    /// </summary>
    public class DomainToApiModelMapper : IMapper<BottleDomainModel, BottleApiModel>
    {
        /// <summary>
        /// Maps a Bottle domain model to an API model.
        /// </summary>
        /// <param name="from">Domain model to map from.</param>
        /// <returns>API model that results from the mapping.</returns>
        public BottleApiModel MapOne(BottleDomainModel from)
        {
            if (from == null) return null;

            return new BottleApiModel
            {
                BottleId = from.BottleId,
                Name = from.Name,
                Region = from.Region,
                AlcoholCategory = from.AlcoholCategory.ToString()
            };
        }

        /// <summary>
        /// Maps an IEnumerable of domain models to an IEnumerable of API models.
        /// </summary>
        /// <param name="from">The IEnumerable of domain models from which to Map.</param>
        /// <returns>IEnumumerable of API models, each mapped from a domain model in the input.</returns>
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
