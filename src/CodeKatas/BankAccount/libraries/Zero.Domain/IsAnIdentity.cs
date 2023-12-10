namespace Zero.Domain;

public abstract class Identity<T> : ValueObject<Identity<T>>, IsAnIdentity
{
    public T Id { get; protected set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }

    public override string ToString()
    {
        return $"{this.GetType().Name} - {Id.ToString()}";
    }
}

public interface IsAnIdentity
{
}