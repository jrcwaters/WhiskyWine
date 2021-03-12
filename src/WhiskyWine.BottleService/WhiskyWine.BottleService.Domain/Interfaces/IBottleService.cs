using System.Collections.Generic;
using System.Threading.Tasks;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.Domain.Interfaces
{
    /// <summary>
    /// Interface that specifies the contract that must be met by classes acting as Bottle domain service.
    /// </summary>
    public interface IBottleService
    {
        /// <summary>
        /// Gets a Bottle entity by its Id.
        /// </summary>
        /// <param name="bottleId">The id of the Bottle to get.</param>
        /// <returns>Task of Bottle containing the Bottle data matching the id.</returns>
        Task<Bottle> GetBottleAsync(string bottleId);

        /// <summary>
        /// Gets all Bottles on record.
        /// </summary>
        /// <returns>Task of IEnumerable containing all Bottles found.</returns>
        Task<IEnumerable<Bottle>> GetAllBottlesAsync();

        /// <summary>
        /// Posts a new Bottle.
        /// </summary>
        /// <param name="bottle">The Bottle object to post.</param>
        /// <returns>Task of Bottle returning the Bottle data that was posted.</returns>
        Task<Bottle> PostBottleAsync(Bottle bottle);

        /// <summary>
        /// Updates an existing Bottle on record.
        /// </summary>
        /// <param name="bottleId">The id of the Bottle on record to update.</param>
        /// <param name="bottle">The new Bottle data to associate to the given id.</param>
        Task UpdateBottleAsync(string bottleId, Bottle bottle);

        /// <summary>
        /// Deletes a Bottle on record.
        /// </summary>
        /// <param name="bottleId">The id of the Bottle on record to delete.</param>
        /// <returns>Task of bool containg true or false depending on success.</returns>
        Task<bool> DeleteBottleAsync(string bottleId);
    }
}
