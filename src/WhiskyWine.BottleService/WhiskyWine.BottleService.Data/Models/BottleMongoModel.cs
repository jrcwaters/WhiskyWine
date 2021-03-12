using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WhiskyWine.BottleService.Data.Models
{
    /// <summary>
    /// The bottle model used for interaction with mongodb.
    /// </summary>
    public class BottleMongoModel
    {
        [BsonId]
        public ObjectId BottleId { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string AlcoholCategory { get; set; }

    }
}
