namespace HRLeaveManagement.Domain.Leave.LeaveRequest.CheckerContracts;

public interface INoAnotherLeaveRequestExistsInPeriodChecker
{
    Task<bool> IsEligible(Employee.Employee requestingEmployee, DateOnly leaveStartedAt, DateOnly leaveEndedAt);
}