using BankAccount.Domain.Accounts;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace BankAccount.Infrastructure;

public class TransactionsConverter : ValueConverter<Transactions, string>
{
    public TransactionsConverter()
        : base(
            v
                => JsonConvert.SerializeObject(v),
            v
                => JsonConvert.DeserializeObject<Transactions>(v))
    {
    }
}


public class MyAccount
{
    public long Id { get; set; }
    
     public Money Balance { get; set; }
    
    //public decimal Amount { get; init; }
    //public string Currency { get; }


}

// | Id | Money|
// | 1  | {amount:10 , currency:Rial}     | 


// | Id | Money |
// | 1  | {amount:10 , currency:Rial}     | 
