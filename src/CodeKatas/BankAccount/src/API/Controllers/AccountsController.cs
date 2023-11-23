
using BankAccount.ApplicationServices;
using BankAccount.ApplicationServices.Query;
using Microsoft.AspNetCore.Mvc;
using Zero.Dispatcher.Command;
using Zero.Dispatcher.Query;

namespace Bank.Account.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ICommandDispatcher _dispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public AccountsController(ICommandDispatcher dispatcher, IQueryDispatcher queryDispathcer)
        {
            _dispatcher = dispatcher;
            _queryDispatcher = queryDispathcer;
        }
        
        [HttpPost("{owner}/open/{initialAmount}")]
        public async Task<IActionResult> Open(string owner, decimal initialAmount)
        {
            await _dispatcher.Dispatch(new OpenBankAccountCommand
            {
                Owner = owner,
                InitialAmount = initialAmount
            });
            return Ok();
        }

        [HttpGet("{owner}/Balance")]
        public async Task<IActionResult> Balance(string owner) 
            => Ok(await _queryDispatcher.RunQuery<GetAccountBalanceQuery, decimal>(new GetAccountBalanceQuery(owner)));
    }
}
