using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiskyWine.BottleService.Data.Models
{
    public class BottleMongoModel
    {
        [BsonId]
        public ObjectId BottleId { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string AlcoholCategory { get; set; }

    }
}
