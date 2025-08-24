using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.BoundedEntities;

public class EmployeeWithLeaveAllocations(Employee employee)
{
    internal Employee Employee => employee;
    public IReadOnlyList<LeaveAllocation> LeaveAllocations => Employee.LeaveAllocations;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="leaveAllocation"></param>
    public void AddLeaveAllocation(LeaveAllocation leaveAllocation) => Employee.AddLeaveAllocation(leaveAllocation);
}
