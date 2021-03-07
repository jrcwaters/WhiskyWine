using System.Threading.Tasks;


namespace WhiskyWine.AccountService.Data.Repository
{
    public interface IRepository<T> where T:class
    {
        /// <summary>
        /// Delete an Entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAccount(object id);
        
        /// <summary>
        /// Get an entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetAccount(object id);
        
        /// <summary>
        /// Insert an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> InsertAccount(T entity);
        
        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> UpdateAccount(string Id, T entity);
    }
}