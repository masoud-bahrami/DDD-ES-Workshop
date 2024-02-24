using Zero.Dispatcher.Command;

namespace BankAccount.ApplicationServices;

public class DefineActivityCommand : IsACommand
{
    public string Activity { get; set; }
    public string Tags { get; set; }
}