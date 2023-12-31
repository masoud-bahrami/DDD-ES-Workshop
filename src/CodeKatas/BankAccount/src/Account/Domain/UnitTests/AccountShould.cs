using Bank.Account.Domain.Contracts.Commands;
using BankAccount.Domain.Accounts;
using BankAccount.Domain.Accounts.Services;
using BankAccount.Domain.Services;

namespace BankAccount.Domain.UnitTests
{
    public class AccountShould
    {

        [Fact]
        public void OpenedSuccessfully()
        {
            //InTermsOfAggregateRoot<Account, AccountId>
            // whole object - value object
            const decimal initialAmount = 10000;

            var account = CreateAccount(initialAmount);

            Assert.Equal(Money.Rial(10000), account.Balance());
        }


        [Fact]
        public void RaiseExceptionIfInitialBalanceIsLowerThan10000()
        {
            decimal initialBalance = 9999;

            Action action = () => CreateAccount(initialBalance);

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void AccountEquality()
        {
            var accountId = Guid.NewGuid().ToString();

            var account1 = CreateAccount(accountId, 10000);
            var account2 = CreateAccount(accountId, 20000);

            Assert.Equal(account1, account2);

        }

        private static Account CreateAccount(string accountId = "1", decimal initialAmount = 10000)
        {
            IAccountDomainService accountDomainService = new AccountDomainService(10000);

            return new Account(AccountId.New(accountId),
                new OpenBankAccountCommand
                {
                    InitialAmount = initialAmount
                }
                , accountDomainService, BankFeesDomainService());
        }

        private static IBankFeesDomainService BankFeesDomainService(decimal smsFees = 0, decimal charge = 0)
        {
            return new BankFeesDomainService(new BankFeesViewModel(smsFees, charge));
        }

        private static Account CreateAccount(decimal initialAmount)
        {
            return CreateAccount(Guid.NewGuid().ToString(), initialAmount);
        }

    }
}