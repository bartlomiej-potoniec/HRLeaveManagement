namespace HRLeaveManagement.Domain.Leave.LeaveRequest.CheckerContracts;

public interface ILeaveRequesterHasEnoughRemainingDaysChecker
{
    Task<bool> IsEligible(Guid requestingEmployeeId, int requestingDays, int currentYear, CancellationToken cancellationToken);
}
