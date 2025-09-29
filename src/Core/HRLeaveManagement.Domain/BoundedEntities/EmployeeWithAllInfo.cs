using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.BoundedEntities;

public class EmployeeWithAllInfo(Employee employee)
{
    internal Employee Employee => employee;

    public IReadOnlyList<EmployeeEducation> EmployeeEducations => employee.EmployeeEducations;
    public IReadOnlyList<EmployeeContract> EmployeeContracts => employee.EmployeeContracts;
    public IReadOnlyList<EmployeeExperience> EmployeeExperiences => employee.EmployeeExperiences;

    public IReadOnlyList<LeaveRequest> LeaveRequests => employee.LeaveRequests;
    public IReadOnlyList<LeaveAllocation> LeaveAllocations => employee.LeaveAllocations;
    public IReadOnlyList<WorkRequest> WorkRequests => employee.WorkRequests;
    public IReadOnlyList<DelegationRequest> DelegationRequests => employee.DelegationRequests;
    public IReadOnlyList<ExtraRemoteWorkRequest> ExtraRemoteWorkRequests => employee.ExtraRemoteWorkRequests;
    public IReadOnlyList<OvertimeRequest> OvertimeRequests => employee.OvertimeRequests;
    public IReadOnlyList<RemoteWorkLimit> RemoteWorkLimits => employee.RemoteWorkLimits;
    public IReadOnlyList<TimeRegister> TimeRegisters => employee.TimeRegisters;
}
