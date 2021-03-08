using System.Threading.Tasks;
using WhiskyWine.AccountService.Domain.Models;

namespace WhiskyWine.AccountService.Domain.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Given the accountId, return the account record
        /// </summary>
        /// <param name="accountId">AccountId</param>
        /// <returns>An account record or null</returns>
        public Task<Account> GetAccount(string accountId);

        /// <summary>
        /// Persists the Account object to the repository
        /// </summary>
        /// <param name="account">the Account object</param>
        /// <returns>An account record or null</returns>
        public Task<Account> InsertAccount(Account account);

        /// <summary>
        /// Updates an existing account record given an account id. 
        /// </summary>
        /// <param name="accountId">The AccountId</param>
        /// <param name="account">The Account Object</param>
        /// <returns>An Account object or null</returns>
        public Task<Account> UpdateAccount(string accountId, Account account);

        /// <summary>
        /// Given an AccountId, deletes the record. 
        /// </summary>
        /// <param name="accountId">the Account Id</param>
        /// <returns>a bool, true if successful, false if not</returns>
        public Task<bool> DeleteAccount(string accountId);
    }
}
