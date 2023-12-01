namespace BankAccount.BankFees;

public class BankFees
{
    private BankFees() { }
    public BankFees(decimal smsFees, decimal charges)
    {
        SmsFees = smsFees;
        Charges = charges;
    }

    public decimal SmsFees { get; private set; }
    public decimal Charges { get; }
}