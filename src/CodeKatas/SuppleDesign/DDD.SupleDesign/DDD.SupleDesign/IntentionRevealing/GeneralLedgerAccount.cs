namespace DDD.SuppleDesign.IntentionRevealing;

public class SubLedgerAccount
{
    public string Code { get; set; }
    public string Name { get; set; }
    public GeneralLedgerAccount GeneralLedgerAccount { get; set; }
}

public class GeneralLedgerAccount
{
    public string Code { get; set; }
    public string Name { get; set; }
}