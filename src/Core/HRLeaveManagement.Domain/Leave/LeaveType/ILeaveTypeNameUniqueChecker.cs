namespace HRLeaveManagement.Domain.Leave.LeaveType;

public interface ILeaveTypeNameUniqueChecker
{
    Task<bool> IsEligible(string name, CancellationToken cancellationToken);
}
