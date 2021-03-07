using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace WhiskyWine.AccountService.Data.Repository
{
    public class AccountRepository : IRepository <Domain.Models.Account>
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
            var record = this._cache.TryGetValue(id, out Domain.Models.Account _);

            if (!record) return await Task.FromResult(false);
            this._cache.Remove(id);

            return await Task.FromResult(true);
        }

        /// <summary>
        /// Retrieves a record from the repository
        /// </summary>
        /// <param name="id">The ID of the record to retrieve</param>
        /// <returns>A record or Null</returns>
        public async Task<Domain.Models.Account> GetAccount(object id)
        {
            var cachedValue = this._cache.TryGetValue(id, out Domain.Models.Account result);

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
            Domain.Models.Company entity)
        {
            if (this._cache.TryGetValue(entity.AccountCode, out Domain.Models.Company _))
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
        public async Task<Domain.Models.Account> UpdateAccount(string accountId,
            Domain.Models.Account entity)
        {
            if (!this._cache.TryGetValue(accountId, out Domain.Models.Account _))
            {
                return null;
            }

            entity.AccountCode = accountId;

            this._cache.Set(entity.AccountCode, entity);

            return await Task.FromResult(entity);
        }
    }
}