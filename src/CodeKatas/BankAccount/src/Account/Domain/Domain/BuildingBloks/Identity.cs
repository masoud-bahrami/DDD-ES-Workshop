namespace BankAccount.Domain.NewFolder;

public abstract class Identity<T> : ValueObject<Identity<T>>, Identity
{
    public T Id { get; protected set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }
}

public interface Identity
{
}