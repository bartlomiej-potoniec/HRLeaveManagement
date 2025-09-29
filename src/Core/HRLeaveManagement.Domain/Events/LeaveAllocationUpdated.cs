using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.Events;

public sealed class LeaveAllocationUpdated(LeaveAllocation leaveAllocation) : IEntityEvent
{
    public int Id => leaveAllocation.Id;
    public int? AvailableDays => leaveAllocation.AvailableDays;
    public Guid EmployeeId => leaveAllocation.Employee.Id;
    public string EmployeeFirstName => leaveAllocation.Employee.FirstName;
    public string EmployeeLastName => leaveAllocation.Employee.LastName;
    public string LeaveTypeName => leaveAllocation.LeaveType.Name;

    public DateTime OccurredOn => DateTime.UtcNow;
    public string Content => $"Zaktualizowano przydział urlopu: { LeaveTypeName } dla pracownika {EmployeeId}:{EmployeeFirstName} {EmployeeLastName} " +
        $"o (nową) ilość dni: { AvailableDays }";
}
