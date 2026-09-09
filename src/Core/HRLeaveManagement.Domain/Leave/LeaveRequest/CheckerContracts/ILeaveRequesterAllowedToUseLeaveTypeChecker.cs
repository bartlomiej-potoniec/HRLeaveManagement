namespace HRLeaveManagement.Domain.Leave.LeaveRequest.CheckerContracts;

public interface ILeaveRequesterAllowedToUseLeaveTypeChecker
{
    Task<bool> IsEligible(Employee.Employee requester,
                          LeaveType.LeaveType leaveType,
                          CancellationToken cancellationToken);
}