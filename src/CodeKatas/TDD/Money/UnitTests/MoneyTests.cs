using CodeKata.TDD.Domain;
using FluentAssertions;
using Expression = CodeKata.TDD.Domain.Expression;

namespace CodeKata.TDD
{
    public class MoneyTests
    {
        [Fact]
        public void DollarMultiplication()
        {
            var five = Dollar(5);

            var ten = five.Times(2);

            ten
                .Should()
                .Be(Dollar(10));

            var fifteen = five.Times(3);
            fifteen
                .Should().Be(Dollar(15));
        }

        [Fact]
        public void DollarEquality()
        {
            Assert.Equal(Dollar(10), Dollar(10));
            Assert.NotEqual(Dollar(5), Dollar(10));

            Assert.Equal(Franc(10), Franc(10));
            Assert.NotEqual(Franc(5), Franc(10));


            Franc(5)
                .Should()
                .NotBe(Dollar(5));
        }

        [Fact]
        public void FrancMultiplication()
        {
            var five = Franc(5);

            var ten = five.Times(2);

            ten.Should()
                .Be(Franc(10));

            var fifteen = five.Times(3);
            fifteen
                .Should().Be(Franc(15));
        }

        [Fact]
        public void TestCurrency()
        {
            Money
                .Dollar(5)
                .GetCurrency()
                .Should()
                .Be(Currency.Dollar);

            Money
                .Franc(5)
                .GetCurrency()
                .Should()
                .Be(Currency.Franc);
        }


        [Fact]
        public void DollarTimes()
        {
            var dollar = Dollar(5);

            dollar
                .Times(2)
                .Should().Be(Dollar(10));

            var fiveFrancs = Franc(5);
            fiveFrancs
                .Times(2)
                .Should().Be(Franc(10));
        }

        [Fact]
        public void AddDollars()
        {
            var five = Dollar(5);

            var sum = five.Plus(five);
            var bank = new Bank();

            var reduced = bank.Reduce(sum, Currency.Dollar);
            reduced.Should().Be(Dollar(10));
        }

        [Fact]
        public void PlusShouldReturnSum()
        {
            var five = Dollar(5);
            var result = five.Plus(five);
            var sum = (Sum)result;
            sum.Augend.Should().Be(five);
            sum.Addend.Should().Be(five);
        }

        [Fact]
        public void BankShouldReduceSuccessfullySumExpression()
        {
            Expression sum = new Sum(Dollar(3), Dollar(3));
            var aBank = new Bank();
            var result = aBank.Reduce(sum, Currency.Dollar);
            result.Should().Be(Dollar(6));
        }


        [Fact]
        public void ReduceMoney()
        {
            var bank = new Bank();

            var result = bank.Reduce(Dollar(1), Currency.Dollar);
            result.Should().Be(Dollar(1));


        }



        [Fact]
        public void ReduceMoneyWithDifferentCurrency()
        {
            var bank = new Bank();
            bank.AddRate(Currency.Franc, Currency.Dollar, 2);

            var result = bank.Reduce(Franc(2), Currency.Dollar);
            result.Should().Be(Dollar(1));
        }

        [Fact]
        public void testIdentity()
        {
            new Bank()
                .GetRate(Currency.Dollar, Currency.Dollar)
                .Should().Be(1);

            new Bank()
                .GetRate(Currency.Franc, Currency.Franc)
                .Should().Be(1);
        }
        [Fact]
        public void mixedAddition()
        {
            var fiveDollars = Dollar(5);
            var tenFrancs = Franc(10);
            var bank = new Bank();
            bank.AddRate(Currency.Franc,Currency.Dollar , 2 );

            var reduced = bank.Reduce(fiveDollars.Plus(tenFrancs), Currency.Dollar);

            reduced.Should().Be(Dollar(10));
        }
        
        private static Money Franc(int amount)
        {
            return Money.Franc(amount);
        }

        private static Money Dollar(int amount)
        {
            return Money.Dollar(amount);
        }
    }
}