using DDD.SuppleDesign.Assertions;

namespace DDD.SupleDesign.Tests
{
    using Xunit;

    public class BankAccountTests
    {




        [Fact]
        public void testPreCondition()
        {
            // Withraw=System-Under Test(SUT)
            
            // then = outcome => amount <= balance = balance = 0 , exception will be thrown
            // When  = action  =>  bankAccount.Withraw(amount)
            // if = context => amount = 11 and balance = 10


        }
        
        
        
        


        
        
        
        
        
        
        
        
        [Fact]
        public void BankAccount_Withdraw_ValidAmount()
        {
            // Arrange
            decimal initialBalance = 1000;
            decimal withdrawalAmount = 500;
            var account = new BankAccount(initialBalance);

            // Act
            account.Withdraw(withdrawalAmount);

            // Assert
            decimal expectedBalance = (initialBalance - withdrawalAmount);
            Assert.Equal(expectedBalance, account.Balance());
        }

        [Fact]
        public void BankAccount_Withdraw_InsufficientBalance()
        {
            // Arrange
            decimal initialBalance = 1000;
            decimal withdrawalAmount = 1500;
            var account = new BankAccount(initialBalance);

            // Act & Assert
            Assert.Throws<InsufficientBalanceException>(() =>
            {
                account.Withdraw(withdrawalAmount);
            });
        }

        [Fact]
        public void BankAccount_Deposit_ValidAmount()
        {
            // Arrange
            decimal initialBalance = 1000;
            decimal depositAmount = 500;
            var account = new BankAccount(initialBalance);

            // Act
            account.Deposit(depositAmount);

            // Assert
            Assert.Equal(initialBalance + depositAmount, account.Balance);
        }

        [Fact]
        public void BankAccount_Deposit_InvalidAmount()
        {
            // Arrange
            decimal initialBalance = 1000;
            decimal depositAmount = -500;
            var account = new BankAccount(initialBalance);

            // Act & Assert
            Assert.Throws<InvalidDepositAmountException>(() =>
            {
                account.Deposit(depositAmount);
            });
        }
    }

   
}