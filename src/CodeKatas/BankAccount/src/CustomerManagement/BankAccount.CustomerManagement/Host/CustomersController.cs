using BankAccount.CustomerManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankAccount.CustomerManagement.Host;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerServices _customerServices;

    public CustomersController(ICustomerServices customerServices) 
        => _customerServices = customerServices;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RegisterCustomerCommand cmd)
    {
        await _customerServices.Register(cmd);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Get() 
        => Ok(await _customerServices.GetAll());
}

 // vertical slicing