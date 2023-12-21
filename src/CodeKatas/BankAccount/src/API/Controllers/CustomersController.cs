using BankAccount.CustomerManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Account.API.Controllers;

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
    {
        
        return Ok(await _customerServices.GetAll());
    }
}