using System.Threading.Tasks;
using WhiskyWine.AccountService.Data.Repository;

namespace WhiskyWine.AccountService.Domain.Services
{
    public class IAccountsService
    {
        /// <summary>
        /// Given the accountId, return the account record
        /// </summary>
        /// <param name="accountId">AccountId</param>
        /// <returns>An account record or null</returns>
        public Task<Models.Account> GetAccount(string accountId);
        
        /// <summary>
        /// Persists the Account object to the repository
        /// </summary>
        /// <param name="account">the Account object</param>
        /// <returns>An account record or null</returns>
        public Task<Models.Account> InsertAccount(Models.Account account);
        
        /// <summary>
        /// Updates an existing account record given an account id. 
        /// </summary>
        /// <param name="accountId">The AccountId</param>
        /// <param name="account">The Account Object</param>
        /// <returns>An Account object or null</returns>
        public Task<Models.Account> UpdateAccount(string accountId, Models.Account account);
        
        /// <summary>
        /// Given an AccountId, deletes the record. 
        /// </summary>
        /// <param name="accountId">the Account Id</param>
        /// <returns>a bool, true if successful, false if not</returns>
        public Task<bool> DeleteAccount(string accountId);
    }
}