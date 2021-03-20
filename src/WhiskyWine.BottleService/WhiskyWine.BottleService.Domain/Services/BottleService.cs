using System.Collections.Generic;
using System.Threading.Tasks;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.Domain.Services
{
    /// <summary>
    /// The main domain service for bottle entities. Holds the domain logic for bottles and communicates messages passed to and from ports, like web APIs and repositories.
    /// </summary>
    public class BottleService : IBottleService
    {
        /// <summary>
        /// A class implementing the IRepository interface, used for communication of bottles with the data store.
        /// </summary>
        private readonly IRepository<BottleDomainModel> _repository;

        /// <summary>
        /// Constructs an instance of the BottleService.
        /// </summary>
        /// <param name="repository"></param>
        public BottleService(IRepository<BottleDomainModel> repository)
        {
            this._repository = repository;
        }

        /// <summary>
        /// Gets a Bottle from the repository using its id.
        /// </summary>
        /// <param name="bottleId">The id of the bottle to get.</param>
        /// <returns>Task of Bottle containing the Bottle returned by the repository.</returns>
        public async Task<BottleDomainModel> GetBottleAsync(string bottleId)
        {
            return await this._repository.GetByIdAsync(bottleId);
        }

        /// <summary>
        /// Gets all Bottles returned by the repository.
        /// </summary>
        /// <returns>Task of IEnumeralbe of Bottle, containing the Bottles returned from the repository.</returns>
        public async Task<IEnumerable<BottleDomainModel>> GetAllBottlesAsync()
        {
            return await this._repository.GetAllAsync();
        }

        /// <summary>
        /// Posts a new Bottle to the repository.
        /// </summary>
        /// <param name="bottle">The Bottle object to post.</param>
        /// <returns>Task of Bottle containing the Bottle that has been posted.</returns>
        public async Task<BottleDomainModel> PostBottleAsync(BottleDomainModel bottle)
        {
            return await this._repository.InsertAsync(bottle);
        }

        /// <summary>
        /// Updates an existing Bottle in the repository.
        /// </summary>
        /// <param name="bottleId">The id of the Bottle to update.</param>
        /// <param name="bottle">The new Bottle to associate to the given id.</param>
        public async Task UpdateBottleAsync(string bottleId, BottleDomainModel bottle)
        {
            await this._repository.UpdateAsync(bottleId, bottle);
        }

        /// <summary>
        /// Deletes a Bottle object from the repository.
        /// </summary>
        /// <param name="bottleId">The id of the Bottle to delete.</param>
        /// <returns>Task of bool, indicating the success of the deletion.</returns>
        public async Task<bool> DeleteBottleAsync(string bottleId)
        {
            return await this._repository.DeleteAsync(bottleId);
        }
    }
}
