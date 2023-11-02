
using BankAccount.ApplicationServices;
using BankAccount.ApplicationServices.Dispatcher;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Account.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ICommandDispatcher _dispatcher;


        public AccountsController(ICommandDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
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
        {
            return Ok(10000);
        }
    }
}
