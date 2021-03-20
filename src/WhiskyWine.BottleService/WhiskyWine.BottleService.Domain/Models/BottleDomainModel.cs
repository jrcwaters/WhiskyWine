
using WhiskyWine.BottleService.Domain.Enums;

namespace WhiskyWine.BottleService.Domain.Models
{
    /// <summary>
    /// The main domain model for the service.
    /// </summary>
    public class BottleDomainModel
    {
        /// <summary>
        /// Property to hold the bottle identifier. 
        /// Stored as a string rather than using any technology specific Id type (eg ObjectId for Mongo) to ensure Core project is truly blind to persistence implementation.
        /// Will be mapped to such a storage model with such a type in persistence project if necessary.
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
        public AlcoholCategory AlcoholCategory { get; set; }
    }
}
