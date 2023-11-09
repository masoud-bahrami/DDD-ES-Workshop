using BankAccount.ApplicationServices.Dispatcher;

namespace BankAccount.ApplicationServices;

public class OpenBankAccountCommand : IsACommand
{
    public string Owner { get; set; }
    public decimal InitialAmount { get; set; }
}