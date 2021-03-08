using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using WhiskyWine.AccountService.Domain.Interfaces;
using WhiskyWine.AccountService.Domain.Models;

namespace WhiskyWine.AccountService.Data.Repositories
{
    public class AccountRepository : IRepository <Account>
    {
        private readonly IMemoryCache _cache;

        public AccountRepository(IMemoryCache memoryCache)
        {
            this._cache = memoryCache;
        }
        
        /// <summary>
        /// Deletes a record from the repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAccount(object id)
        {
            var record = this._cache.TryGetValue(id, out Account _);

            if (!record) return await Task.FromResult(false);
            this._cache.Remove(id);

            return await Task.FromResult(true);
        }

        /// <summary>
        /// Retrieves a record from the repository
        /// </summary>
        /// <param name="id">The ID of the record to retrieve</param>
        /// <returns>A record or Null</returns>
        public async Task<Account> GetAccount(object id)
        {
            var cachedValue = this._cache.TryGetValue(id, out Account result);

            if (cachedValue)
            {
                return await Task.FromResult(result);
            }

            return null;
        }

        /// <summary>
        /// Inserts a new record into the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Domain.Models.Account> InsertAccount(
            Account entity)
        {
            if (this._cache.TryGetValue(entity.AccountCode, out Account _))
            {
                return null;
            }

            this._cache.Set(entity.AccountCode, entity);

            return await Task.FromResult(entity);
        }

        /// <summary>
        /// Updates an existing record in the repository
        /// </summary>
        /// <param name="accountId">Ths is of the record to update</param>
        /// <param name="entity">The updates to persist</param>
        /// <returns>A record or Null</returns>
        public async Task<Account> UpdateAccount(string accountId, Account entity)
        {
            if (!this._cache.TryGetValue(accountId, out Account _))
            {
                return null;
            }

            entity.AccountCode = accountId;

            this._cache.Set(entity.AccountCode, entity);

            return await Task.FromResult(entity);
        }
    }
}