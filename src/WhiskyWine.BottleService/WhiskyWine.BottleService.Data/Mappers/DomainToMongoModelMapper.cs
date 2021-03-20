using MongoDB.Bson;
using System.Collections.Generic;
using WhiskyWine.BottleService.Data.Models;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.Data.Mappers
{
    /// <summary>
    /// The adapter class used to map from domain Bottle models to BottleMongoModel models used to interact with the mongo database.
    /// This class forms one of the adapters for the ports and adapter architecture of the service.
    /// </summary>
    public class DomainToMongoModelMapper : IMapper<BottleDomainModel, BottleMongoModel>
    {
        /// <summary>
        /// Maps a domain Bottle model to a BottleMongoModel so that bottle data can be written to mongo database.
        /// </summary>
        /// <param name="fromType">The Bottle to map from.</param>
        /// <returns>The BottleMongoModel resulting from the mapping.</returns>
        public BottleMongoModel MapOne(BottleDomainModel from)
        {
            if (from == null) return null;

            //If the Id of the passed Bottle is not parseable to mongo ObjectId set it to empty.
            if (!ObjectId.TryParse(from.BottleId, out ObjectId objectId))
            {
                objectId = ObjectId.Empty;
            }

            return new BottleMongoModel
            {
                BottleId = objectId,
                Name = from.Name,
                Region = from.Region,
                AlcoholCategory = from.AlcoholCategory
            };
        }

        public IEnumerable<BottleMongoModel> MapMany(IEnumerable<BottleDomainModel> from)
        {
            var mappedList = new List<BottleMongoModel>();
            foreach (var bottle in from)
            {
                mappedList.Add(MapOne(bottle));
            }
            return mappedList;
        }
    }
}
