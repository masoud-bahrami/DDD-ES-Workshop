using BankAccount.ApplicationServices;
using Microsoft.AspNetCore.Mvc;
using Zero.Dispatcher.CommandPipeline;

namespace Bank.AccountManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActivityController : ControllerBase
{
    private readonly IAmACommandPipeline _bus;

    public ActivityController(IAmACommandPipeline bus)
    {
        _bus = bus;
    }

    [HttpPost]
    public async Task<IActionResult> Post(DefineActivityCommand command)
    {
        await _bus.Process(command);

        return Ok();
    }
}