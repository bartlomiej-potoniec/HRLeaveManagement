namespace HRLeaveManagement.Domain.Tests.Helpers;

public class EmployeeHelper
{
    public static Employee CreateEmployee()
        => Employee.Create("Logistic", "Logistic work", 1, Guid.NewGuid());
}
