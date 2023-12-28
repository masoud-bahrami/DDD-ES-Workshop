using BankAccount.BankFees.BAL;
using Microsoft.AspNetCore.Mvc;

namespace BankAccount.BankFees.Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BankFeesController : ControllerBase
{
    private readonly IBankFeesServices _bankFeesServices;

    public BankFeesController(IBankFeesServices bankFeesServices)
        => _bankFeesServices = bankFeesServices;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] SetBankFeesCommand cmd)
    {
        await _bankFeesServices.SetFees(cmd);
        return Ok();
    }
}