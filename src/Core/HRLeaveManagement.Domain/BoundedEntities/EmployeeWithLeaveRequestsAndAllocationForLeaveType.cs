using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.BoundedEntities;

public sealed class EmployeeWithLeaveRequestsAndAllocation(Employee employee)
{
    internal Employee Employee => employee;

    public LeaveAllocation LeaveAllocation => Employee.LeaveAllocations[0];
    public IReadOnlyList<LeaveRequest> LeaveRequests => Employee.LeaveRequests;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="leaveRequest"></param>
    public void AddLeaveRequest(LeaveRequest leaveRequest) => Employee.AddLeaveRequest(leaveRequest);
}
