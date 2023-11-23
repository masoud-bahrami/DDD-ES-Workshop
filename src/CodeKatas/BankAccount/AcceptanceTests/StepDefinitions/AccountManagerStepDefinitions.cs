using BankAccount.AcceptanceTests.Drivers.AccountManager;

namespace BankAccount.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class AccountManagerStepDefinitions
    {
        private readonly IBankAccountDriver _driver;

        public AccountManagerStepDefinitions()
        {
            _driver = new BankAccountApiDriver();
        }

        [Given(@"Masoud as a customer")]
        public void GivenPersonAsACustomer()
        {

            // TODO
        }

        [Given(@"There is no any bank account for Masoud")]
        public void GivenThereIsNoAnyBankAccountFor()
        {

        }

        [When(@"(.*) opens a new bank account with (.*) toman")]
        public async Task WhenOpensANewBankAccountWithToman(string owner, decimal initialAmount)
        {
            await _driver.OpenBank(owner: owner, initialAmount: initialAmount);
        }

        [Then(@"A new bank account will be opened for (.*) with (.*) toman balance")]
        public async Task ThenANewBankAccountWillBeOpenedForCustomerWithTomanBalance(string owner, decimal expectedAmount)
        {
            await _driver.AssertThatOwnerHasAnAccountWithInitialBalance(owner, expectedAmount);
        }
    }
}
