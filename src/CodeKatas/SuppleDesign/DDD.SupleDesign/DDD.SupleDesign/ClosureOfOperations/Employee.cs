namespace DDD.SuppleDesign.ClosureOfOperations;

public class Employee
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Employee Supervisor { get; set; }

    public Employee GetSupervisor()
    {
        return Supervisor;
    }
}