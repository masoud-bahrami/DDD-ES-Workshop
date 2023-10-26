
using Microsoft.AspNetCore.Mvc;

namespace Bank.Account.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        [HttpPost("{owner}/open/{initialAmount}")]
        public async Task<IActionResult> Open(string owner, decimal initialAmount)
        {
            return Ok();
        }

        [HttpGet("{owner}/Balance")]
        public async Task<IActionResult> Balance(string owner)
        {
            return Ok(10000);
        }
    }
}
