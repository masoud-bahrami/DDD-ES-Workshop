namespace DDD.SuppleDesign.ConceptualContours;

public class Loan
{
    public string LoanNumber { get; set; }
    public decimal Amount { get; set; }
    public double InterestRate { get; set; }
    public DateTime StartDate { get; set; }
    public bool IsApproved { get; set; }
    public LoanType LoanType { get; set; }

    public decimal CalculateInterest()
    {
        var interest = Amount * (decimal)InterestRate;
        return (decimal)interest;
    }
}

public enum LoanType
{
    ShortTem,
    LongTerm
}