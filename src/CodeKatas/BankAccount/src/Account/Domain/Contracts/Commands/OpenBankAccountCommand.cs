using Zero.Dispatcher.Command;

namespace Bank.Account.Domain.Contracts.Commands;

public class OpenBankAccountCommand : IsACommand
{
    public string Owner { get; set; }
    public decimal InitialAmount { get; set; }
}