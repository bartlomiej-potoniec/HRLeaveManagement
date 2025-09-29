using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.Events;

public sealed class EmployeeCreated(Employee employee, Guid userId) : IEntityEvent
{
    public Guid UserId => userId;
    public Guid EmployeeId => employee.Id;
    public string EmployeeFirstName => employee.FirstName;
    public string EmployeeLastName => employee.LastName;
    public string EmployeePosition => employee.Position;

    public DateTime OccurredOn => DateTime.UtcNow;
    public string Content => $"Utworzono pracownika { EmployeeFirstName } { EmployeeLastName } " +
        $"na stanowisku { EmployeePosition }";
}
