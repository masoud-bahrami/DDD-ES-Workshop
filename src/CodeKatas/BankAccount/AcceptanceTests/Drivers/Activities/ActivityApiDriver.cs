using Newtonsoft.Json;
using System.Text;

namespace BankAccount.AcceptanceTests.Drivers.Activities;

public class ActivityApiDriver : IActivityDriver
{
    private readonly HttpClient _httpClient;

    public ActivityApiDriver(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public void RegisterUser(string sara)
    {
    }

    public void DefineActivity(string activity, string tags)
    {
        var command = new DefineActivityCommand
        {
            Activity = activity,
            Tags = tags
        };

        var response = _httpClient.PostAsync("api/Activity", Content(command)).Result;


        response.EnsureSuccessStatusCode();
    }

    public void AssertActivities(Table table)
    {
        throw new NotImplementedException();
    }

    private static StringContent Content(DefineActivityCommand command)
    {
        return new StringContent(JsonConvert.SerializeObject(command)
            , Encoding.UTF8, "application/json");
    }
}