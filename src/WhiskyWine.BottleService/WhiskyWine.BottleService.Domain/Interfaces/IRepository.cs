using System.Collections.Generic;
using System.Threading.Tasks;

namespace WhiskyWine.BottleService.Domain.Interfaces
{
    /// <summary>
    /// Interface that specifies the contract that must be met by classes acting as repositories that communicate with data stores.
    /// Generic type parameter T specifies the type of the object that will be read and written from the data store.
    /// </summary>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Inserts one entity into the data store.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>Task of T, where T is the type of the object that has been inserted.</returns>
        Task<T> InsertAsync(T entity);

        /// <summary>
        /// Gets one entity from the data store using its id.
        /// </summary>
        /// <param name="id">The id of the object to get.</param>
        /// <returns>Task of T containing the object retrieved from the data store, or null if no object retrieved.</returns>
        Task<T> GetByIdAsync(string id);

        /// <summary>
        /// Gets all objects from the data store.
        /// </summary>
        /// <returns>Task of IEnumerable of T, containing the objects returned from the data store.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Updates an object in the data store.
        /// </summary>
        /// <param name="id">The id of the object to be updated.</param>
        /// <param name="entity">The new object to associate to the id.</param>
        Task UpdateAsync(string id, T entity);

        /// <summary>
        /// Deletes an object from the data store.
        /// </summary>
        /// <param name="id">The id of the object to delete.</param>
        /// <returns>Task of bool, signifying whether or not the deletion was a success.</returns>
        Task<bool> DeleteAsync(string id);
    }
}
