using BankAccount.Domain.Accounts;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace BankAccount.Infrastructure;

public class TransactionsConverter : ValueConverter<Transactions, string>
{
    public TransactionsConverter()
        : base(
            v => JsonConvert.SerializeObject(v),
            v => JsonConvert.DeserializeObject<Transactions>(v))
    {
    }
}