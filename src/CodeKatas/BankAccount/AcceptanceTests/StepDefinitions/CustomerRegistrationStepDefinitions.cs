using BankAccount.AcceptanceTests.Drivers.CustomerRegistration;

namespace BankAccount.AcceptanceTests.StepDefinitions;

[Binding]
public class CustomerRegistrationStepDefinitions
{
    private ICustomerRegistrationDriver _customerRegistrationDriver;

    public CustomerRegistrationStepDefinitions(ICustomerRegistrationDriver customerRegistrationDriver)
        => _customerRegistrationDriver = customerRegistrationDriver;

    [Given(@"The latest given customer id is (.*)")]
    public async Task GivenTheLatestGivenCustomerIdIs(string latestGivenCustomerNumber)
    {
        
    }

    [When(@"A new customer called (.*) enroll in bank with following information")]
    public async Task  WhenANewCustomerEnrollInBankWithFollowingInformation(string customerName, Table table)
    {
        await _customerRegistrationDriver.Register(customerName, table);
    }

    [Then(@"We will give him customer id (.*) which he can use for further services")]
    public async Task ThenWeWillGiveHimCustomerIdWhichHeCanUseForFurtherServices(string customerId)
    {
        await _customerRegistrationDriver.AssertCustomerId(customerId);

    }

}