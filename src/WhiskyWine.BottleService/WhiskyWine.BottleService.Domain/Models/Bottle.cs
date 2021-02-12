using WhiskyWine.BottleService.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WhiskyWine.BottleService.Domain.Models
{
    public class Bottle
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BottleId { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public AlcoholCategory AlcoholCategory { get; set; }
    }
}
