using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.BoundedEntities;

public sealed class EmployeeWithLeaveRequests(Employee employee)
{
    internal Employee Employee => employee;

    public IReadOnlyList<LeaveRequest> LeaveRequests => Employee.LeaveRequests;
}
