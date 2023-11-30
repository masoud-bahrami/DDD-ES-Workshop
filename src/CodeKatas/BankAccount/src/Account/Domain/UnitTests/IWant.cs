using BankAccount.Domain.Accounts;

namespace BankAccount.Domain.UnitTests
{
    public class IWant
    {
        
        [Fact]
        public void moneisWithTheSameAmountAndCurencyToBeEqualEachOther()
        {
            decimal initialAmount = 10000;
            var money = Money.Rial(initialAmount);

            Assert.Equal(Money.Rial(10000), money);
        }

        [Fact]
        public void moneyDontAcceptAnyAmountLowerThanZeri()
        {
            Action action=()=> Money.Rial(-1);
            Assert.Throws<Money.AmountIsLowerThanZeroException>(action);
        }
    }
}