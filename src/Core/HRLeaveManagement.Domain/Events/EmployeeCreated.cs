using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.Events;

public sealed class EmployeeCreated(Employee employee) : IEntityEvent
{
    public Employee Employee => employee;

    public DateTime OccurredOn => DateTime.UtcNow;
    public string Content => "Employee Created";
}
