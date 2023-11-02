namespace BankAccount.Domain.UnitTests
{
    public class AccountShould
    {
        [Fact]
        public void OpenedSuccessfully()
        {
            decimal initialAmount = 10000;
            
            var account = new Account(initialAmount);

            Assert.Equal(10000, account.Amount);
        }
    }
}