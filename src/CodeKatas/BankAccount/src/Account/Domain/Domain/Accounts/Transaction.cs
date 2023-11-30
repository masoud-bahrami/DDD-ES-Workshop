using Zero.Domain;

namespace BankAccount.Domain.Accounts;

public abstract class Transaction : ValueObject<Transaction>
{
    public DateTime OccurredAt { get; private set; }
    public Money Money { get; private set; }
    public TransactionType Type { get; private set; }

    protected Transaction(Money money, TransactionType type)
    {
        OccurredAt = DateTime.Now;
        Money = money;
        Type = type;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return OccurredAt;
        yield return Money;
    }
}

public abstract class WithdrawalTransaction : Transaction
{
    internal WithdrawalTransaction(Money money) : base(money, TransactionType.Withdrawal)
    {
    }
}

public abstract class DepositTransaction : Transaction
{
    internal DepositTransaction(Money money)
        : base(money, TransactionType.Deposit)
    {
    }
}

public class OpeningAccountTransaction : DepositTransaction
{
    public static OpeningAccountTransaction New(Money initialAmount)
        => new(initialAmount);

    private OpeningAccountTransaction(Money money) : base(money)
    {
    }
}

public class SmsFeesTransaction : WithdrawalTransaction
{
    public static SmsFeesTransaction New(Money fee)
        => new(fee);

    private SmsFeesTransaction(Money money) : base(money)
    {
    }
}

public class BankFeesTransaction : WithdrawalTransaction
{
    public static BankFeesTransaction New(Money fee)
        => new(fee);

    private BankFeesTransaction(Money money) : base(money)
    {
    }
}