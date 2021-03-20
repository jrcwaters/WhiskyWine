using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WhiskyWine.BottleService.Domain.Enums;

namespace WhiskyWine.BottleService.Data.Models
{
    /// <summary>
    /// The bottle model used for interaction with mongodb.
    /// </summary>
    public class BottleMongoModel
    {
        /// <summary>
        /// The Id of the bottle, stored as an ObjectId to allow automatic creation of unique Id whenever a new bottle is posted to the Mongo DB.
        /// This property is stored as a string in the domain model. ObjectId mapped to and from string when mapping to domain model.
        /// </summary>
        [BsonId]
        public ObjectId BottleId { get; set; }

        /// <summary>
        /// Property to hold the Name of the bottle.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Property to hold the region the bottle was produced in.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Property to hold the category of alcohol the bottle belongs to, for example whisky or wine.
        /// </summary>
        public AlcoholCategory AlcoholCategory { get; set; }

    }
}
