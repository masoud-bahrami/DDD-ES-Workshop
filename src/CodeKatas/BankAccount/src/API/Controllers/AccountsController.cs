
using Bank.Account.Domain.Contracts.Commands;
using Bank.Account.Domain.Contracts.Events;
using BankAccount.ApplicationServices.Query;
using Microsoft.AspNetCore.Mvc;
using Zero.Dispatcher.CommandPipeline;
using Zero.Dispatcher.QueryPipeline;

namespace Bank.Account.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        //private readonly ICommandDispatcher _dispatcher;
        private readonly IAmACommandPipeline _pipeline;
        private readonly IAmQueryHandlerStage _queryHandlerStage;
        //private readonly IQueryDispatcher _queryDispatcher;

        public AccountsController(IAmACommandPipeline pipeline, IAmQueryHandlerStage queryHandlerStage)
        {
            //_dispatcher = dispatcher;
          //  _queryDispatcher = queryDispathcer;
            _pipeline = pipeline;
            _queryHandlerStage = queryHandlerStage;
        }
        
        [HttpPost("{owner}/open/{initialAmount}")]
        public async Task<IActionResult> Open(string owner, decimal initialAmount)
        {
            var id = "";

            _pipeline.IWantToListenTo<ANewAccountHasBeenOpenedDomainEvent>((e, ev, v) =>
            {
                id = ((ANewAccountHasBeenOpenedDomainEvent)e).Id;
            });

            await _pipeline.Process(new OpenBankAccountCommand
            {
                Owner = owner,
                InitialAmount = initialAmount
            });

            return Ok(id);
        }

        [HttpGet("{owner}/Balance")]
        public async Task<IActionResult> Balance(string owner) 
            //=> Ok(await _queryDispatcher.RunQuery<GetAccountBalanceAmAQuery, decimal>(new GetAccountBalanceAmAQuery(owner)));
            => Ok(await _queryHandlerStage.RuneQuery<GetAccountBalanceAmAQuery, decimal>(new GetAccountBalanceAmAQuery(owner)));
    }
}
