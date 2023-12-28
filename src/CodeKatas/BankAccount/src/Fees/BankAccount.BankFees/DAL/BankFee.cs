namespace BankAccount.BankFees.DAL;

public class BankFee
{
    private BankFee() { }
    public BankFee(decimal smsFees, decimal charges)
    {
        SmsFees = smsFees;
        Charges = charges;
    }

    public decimal SmsFees { get; private set; }
    public decimal Charges { get; }
}