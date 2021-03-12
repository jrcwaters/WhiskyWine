
namespace WhiskyWine.BottleService.Domain.Models
{
    /// <summary>
    /// The main domain model for the service.
    /// </summary>
    public class Bottle
    {
        /// <summary>
        /// Property to hold the bottle identifier.
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
        /// </summary>
        public string AlcoholCategory { get; set; }
    }
}
