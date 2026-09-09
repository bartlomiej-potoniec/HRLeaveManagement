namespace HRLeaveManagement.Domain.Leave.LeaveRequest.CheckerContracts;

public interface ILeaveRequestApproverSuperiorOfEmployeeChecker
{
    Task<bool> IsEligible(Guid approverId, Guid requestingEmployeeId, CancellationToken cancellationToken);
}
