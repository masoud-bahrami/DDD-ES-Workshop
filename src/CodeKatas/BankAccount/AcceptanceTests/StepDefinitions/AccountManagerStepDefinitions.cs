using System.Net.NetworkInformation;
using BankAccount.AcceptaceTests.Drivers;
using BankAccount.AcceptaceTests.Drivers.AccountManager;

namespace BankAccount.AcceptaceTests.StepDefinitions
{
    [Binding]
    public class AccountManagerStepDefinitions
    {
        private IBankAccountDriver _driver;

        public AccountManagerStepDefinitions()
        {
            _driver = new BankAccountApiDriver();
        }

        [Given(@"Masoud as a customer")]
        public void GivenMasoudAsACustomer()
        {
            // TODO
        }

        [Given(@"There is no any bank account for Masoud")]
        public void GivenThereIsNoAnyBankAccountForMasoud()
        {

        }

        [When(@"(.*) opens a new bank account with (.*) toman")]
        public async Task WhenOpensANewBankAccountWithToman(string owner, decimal initialAmount)
        {
            await _driver.OpenBank(owner: owner, initialAmount: initialAmount);
        }

        [Then(@"A new bank account will be opened for (.*) with (.*) toman balance")]
        public async Task ThenANewBankAccountWillBeOpenedForMasoudWithTomanBalance(string owner, decimal expectedAmount)
        {
            await _driver.AssertThatOwnerHasAnAccountWithInitialBalance(owner, expectedAmount);
        }
    }
}
