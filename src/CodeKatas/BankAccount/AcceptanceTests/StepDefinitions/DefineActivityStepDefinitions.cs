using BankAccount.AcceptanceTests.Drivers.Activities;

namespace BankAccount.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class DefineActivityStepDefinitions
    {
        private readonly IActivityDriver _driver;
        private string _activity;

        public DefineActivityStepDefinitions(IActivityDriver driver)
        {
            _driver = driver;
        }

        [Given(@"'([^']*)' is a User")]
        public void GivenIsAUser(string sara)
        {
            _driver.RegisterUser(sara);
        }

        [Given(@"her activity list is empty")]
        public void GivenHerActivityListIsEmpty()
        {
        }

        [When(@"she writes an activity with text '([^']*)'")]
        public void WhenSheWritesAnActivityWithText(string activity)
        {
            _activity = activity;
        }

        [When(@"she assigned following tags '([^']*)'")]
        public void WhenSheAssignedFollowingTags(string tags)
        {
            _driver.DefineActivity(_activity, tags);
        }

        [Then(@"she activities should contain following information")]
        public void ThenSheActivitiesShouldContainFollowingInformation(Table table)
        {
            _driver.AssertActivities(table);
        }
    }
}
