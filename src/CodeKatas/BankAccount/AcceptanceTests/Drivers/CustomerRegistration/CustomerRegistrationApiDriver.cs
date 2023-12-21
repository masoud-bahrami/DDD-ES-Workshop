using System.Text;
using Bank.Account.API.Controllers;
using BankAccount.CustomerManagement.Services;
using Newtonsoft.Json;

namespace BankAccount.AcceptanceTests.Drivers.CustomerRegistration;

public class CustomerRegistrationApiDriver : ICustomerRegistrationDriver
{
    private readonly HttpClient _httpClient;

    public CustomerRegistrationApiDriver(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Register(string customerName, Table table)
    {
        //| First Name | Last Name | National Code | Birth Date | Gender |
        var firstName = table.Rows[0]["First Name"];
        var lastName = table.Rows[0]["Last Name"];
        var nationalCode = table.Rows[0]["National Code"];
        var birthDate =DateTime.Parse(table.Rows[0]["Birth Date"]);

        var registerCustomerCommand = new RegisterCustomerCommand(firstName, lastName, nationalCode, birthDate);

        await _httpClient.PostAsync("api/customers", Content(registerCustomerCommand));

    }

    public async Task AssertCustomerId(string customerId)
    {
        var httpResponseMessage = await _httpClient.GetAsync("api/customers");

        var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();
        var customers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CustomerViewModel>>(readAsStringAsync);

        customers.Single()
            .CustomerId.Should().Be(customerId);
    }
    

    private static StringContent Content(object bodyContent) 
        => new(JsonConvert.SerializeObject(bodyContent), Encoding.UTF8, "application/json");
}