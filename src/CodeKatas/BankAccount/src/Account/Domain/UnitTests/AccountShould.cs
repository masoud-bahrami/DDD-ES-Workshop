

using BankAccount.Domain.Services;

namespace BankAccount.Domain.UnitTests
{
    public class AccountShould
    {
        [Fact]
        public void OpenedSuccessfully()
        {
            // whole object - value object
            decimal initialAmount = 10000;

            var account = CreateAccount(initialAmount);

            Assert.Equal(10000, account.Amount);
        }


        [Fact]
        public void RaiseExceptionIfInitialBalanceIsLowerThan10000()
        {
            decimal initialBalance = 9999;

            Action action = () => CreateAccount(initialBalance);

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void Money()
        {
            decimal initialAmount = 10000;
            Money money = Domain.Money.Rial(initialAmount);

            Assert.Equal(Domain.Money.Rial(10000), money);
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

            return new Account(accountId, initialAmount , accountDomainService);
        }

        private static Account CreateAccount(decimal initialAmount)
        {
            return CreateAccount(Guid.NewGuid().ToString(), initialAmount);
        }

    }
}