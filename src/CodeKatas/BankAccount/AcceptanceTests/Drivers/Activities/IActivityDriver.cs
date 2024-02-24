namespace BankAccount.AcceptanceTests.Drivers.Activities;

public interface IActivityDriver
{
    void RegisterUser(string sara);
    void DefineActivity(string activity, string tags);
    void AssertActivities(Table table);
}