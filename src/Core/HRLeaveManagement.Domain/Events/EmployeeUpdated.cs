using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.Events;

public sealed class EmployeeUpdated(Employee employee) : IEntityEvent
{
    public Guid EmployeeId => employee.Id;
    public string EmployeeFirstName => employee.FirstName;
    public string EmployeeLastName => employee.LastName;
    public string EmployeePosition => employee.Position;

    public DateTime OccurredOn => DateTime.UtcNow;
    public string Content => $"Zaktualizowano pracownika z nr.: { EmployeeId }:{ EmployeeFirstName } { EmployeeLastName } " +
        $"na (nowym) stanowisku { EmployeePosition }";
}
