using System.Threading.Tasks;
using WhiskyWine.AccountService.Data.Repository;

namespace WhiskyWine.AccountService.Domain.Services
{
    /// <summary>
    /// The Account Service, used to interact with the repository
    /// </summary>
    public class AccountsService : IAccountsService
    {
        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Models.Account> _accountRepository;

        /// <summary>
        /// The account service constructor
        /// </summary>
        /// <param name="accountRepository">The Account Repository</param>
        public AccountsService(IRepository<Models.Account> accountRepository)
        {
            this._accountRepository = accountRepository;
        }

        /// <summary>
        /// Given the AccountId, return the Account Record
        /// </summary>
        /// <param name="accountId">AccountID</param>
        /// <returns>An account record or null</returns>
        public async Task<Models.Account> GetAccount(string accountId)
        {
            return await _accountRepository.GetAccount(accountId);
        }

        /// <summary>
        /// Persists the account object to the repository
        /// </summary>
        /// <param name="account"></param>
        /// <returns>an account object or null</returns>
        public async Task<Models.Account> InsertAccount(Models.Account account)
        {
            return await _accountRepository.InsertAccount(account);
        }

        /// <summary>
        /// Updates an existing account record given an account id. 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="account"></param>
        /// <returns>An account object or Null </returns>
        public async Task<Models.Account> UpdateAccount(string accountId, Models.Account account)
        {
            return await _accountRepository.UpdateAccount(accountId, account);
        }

        /// <summary>
        /// Given an accountId, Deletes the record. 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>a bool, true if successful, false if not/returns>
        public async Task<bool> DeleteAccount(string accountId)
        {
            return await _accountRepository.DeleteAccount(accountId);
        }
    }
}