using System.Text.Json.Serialization;
using WhiskyWine.BottleService.Domain.Enums;

namespace WhiskyWine.BottleService.Domain.Models
{
    public class Bottle
    {
        [JsonIgnore]
        public int BottleId { get; set; }

        /// <summary>
        /// MongoDB alias for BottleId
        /// </summary>
        [JsonIgnore]
        public int _id => BottleId;
        public string Name { get; set; }
        public string Region { get; set; }
        public AlcoholCategory AlcoholCategory { get; set; }
    }
}
