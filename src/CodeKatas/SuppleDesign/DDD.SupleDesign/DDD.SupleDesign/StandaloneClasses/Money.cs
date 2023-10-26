namespace DDD.SuppleDesign.StandaloneClasses;


public class Account
{
    public decimal Balance { get; set; }
    public string Currency { get; set; }
    public string Owenr  { get; set; }
}

public class Money
{
    public string Currency { get; private set; }
    public decimal Amount { get; private set; }
}