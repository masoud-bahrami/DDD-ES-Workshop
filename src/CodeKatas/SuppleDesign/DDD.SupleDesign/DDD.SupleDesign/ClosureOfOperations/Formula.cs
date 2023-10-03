namespace DDD.SuppleDesign.ClosureOfOperations;

public abstract class Formula
{

    public abstract int Reduce();
}

public class AddFormula : Formula
{
    private Formula Augend;
    private Formula Addend;

    public AddFormula(Formula augend, Formula addend)
    {
        Augend = augend;
        Addend = addend;
    }

    public override int Reduce()
    {
        return Augend.Reduce() + Addend.Reduce();
    }
}
public class DivideFormula : Formula
{

    public Formula FirstArgs { get; }
    public Formula SecondArgs { get; }

    public DivideFormula(Formula firstArgs, Formula secondArgs)
    {
        FirstArgs = firstArgs;
        SecondArgs = secondArgs;
    }

    public override int Reduce()
    {
        return FirstArgs.Reduce() / SecondArgs.Reduce();
    }
}
public class MultiplyFormula : Formula
{

    public override int Reduce()
    {
        throw new NotImplementedException();
    }
}
public class PowFormula : Formula
{

    public override int Reduce()
    {
        throw new NotImplementedException();
    }
}

public class IntFormula : Formula
{
    public int InitialValue { get; }

    public IntFormula(int initialValue)
    {
        InitialValue = initialValue;
    }


    public override int Reduce()
    {
        return InitialValue;
    }
}