
namespace WhiskyWine.BottleService.API.Models
{
    /// <summary>
    /// The Bottle model used for the communication of Bottle data in and out of the API, via the BottlesController.
    /// All properties are strings to allow clients to send and receive Bottle data as easily understandable JSON objects.
    /// </summary>
    public class BottleApiModel
    {
        /// <summary>
        /// Property to hold the bottle identifier, passed to the API as a string.
        /// </summary>
        public string BottleId { get; set; }

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
        /// API model uses string rather than enum to make API calls and responses more understandable. Will be mapped to Enum when converted to domain model.
        /// This should be validated against the AlcoholCategory enum before attempting to parse.
        /// </summary>
        public string AlcoholCategory { get; set; }
    }
}
