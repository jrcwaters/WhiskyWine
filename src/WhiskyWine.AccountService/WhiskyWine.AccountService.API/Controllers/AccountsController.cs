using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WhiskyWine.AccountService.Domain.Interfaces;

namespace WhiskyWine.AccountService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountsService;


        public AccountsController(IAccountService accountsService)
        {
            _accountsService = accountsService;
        }

        [HttpGet("{accountId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAccount(string accountId)
        {
            try
            {
                var result = await this._accountsService.GetAccount(accountId);

                return result == null ? (ActionResult) NotFound(accountId) : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Domain.Models.Account>> PostAccount(string accountId,
            Domain.Models.Account account)
        {
            account.AccountCode = accountId;

            try
            {
                var result = await this._accountsService.InsertAccount(account);

                if (result != null)
                {
                    return Created($"api/accounts/{account.AccountCode}", account);
                }

                return BadRequest("Cannot insert duplicate record.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Domain.Models.Account>> UpdateAccount(string accountId,
            Domain.Models.Account account)
        {
            account.AccountCode = accountId;

            try
            {
                var result = await this._accountsService.UpdateAccount(accountId, account);

                if (result != null)
                {
                    return Ok(account);
                }

                return NotFound(accountId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteAccount(string accountId)
        {
            try
            {
                var deleted = await this._accountsService.DeleteAccount(accountId);

                if (deleted)
                {
                    return Ok();
                }

                return NotFound(accountId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}