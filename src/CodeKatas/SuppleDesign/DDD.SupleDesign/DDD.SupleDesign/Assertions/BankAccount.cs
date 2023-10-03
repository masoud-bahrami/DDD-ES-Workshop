using System.Diagnostics.Contracts;

namespace DDD.SuppleDesign.Assertions;


// Design by Contract
// 
// Business Rules = Acceptance Criteria
// Pre conditions
// Invariants
// Post condition

public class BankAccount
{
    private decimal balance;
    

    public BankAccount(decimal initialBalance)
    {
        // Precondition: The initial balance should be non-negative
        Contract.Requires<ArgumentException>(initialBalance >= 0, "Initial balance cannot be negative.");

        this.balance = initialBalance;
    }


    public void Deposit(decimal amount)
    {
        // Precondition: The deposit amount should be positive
        Contract.Requires<ArgumentException>(amount > 0, "Deposit amount should be positive.");

        // Postcondition: The balance should increase by the deposit amount
        Contract.Ensures(balance == Contract.OldValue(balance) + amount, "Balance should increase by the deposit amount.");

        balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        // Precondition: The withdrawal amount should be positive and not exceed the balance
        Contract.Requires<ArgumentException>(amount > 0 && amount <= balance, "Invalid withdrawal amount.");

        // Postcondition: The balance should decrease by the withdrawal amount
        Contract.Ensures(balance == Contract.OldValue(balance) - amount, "Balance should decrease by the withdrawal amount.");

        balance -= amount;
    }
    public decimal Balance() 
        => balance;

    // Invariant: The balance should never be negative
    [ContractInvariantMethod]
    private void BalanceInvariant()
    {
        Contract.Invariant(balance >= 0);
    }
}




public class InvalidDepositAmountException : Exception
{
}

public class InsufficientBalanceException : Exception
{
}