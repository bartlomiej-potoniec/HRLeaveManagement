namespace HRLeaveManagement.Domain.Tests.Helpers;

public class WorkRequestTestHelper : WorkRequest
{
    public void InitializeBase(Guid requestingEmployeeId,
                               DateOnly startedAt,
                               DateOnly endedAt,
                               Guid approverId,
                               string? approverComment = null)
        => 
            base.InitializeBase(requestingEmployeeId, startedAt, endedAt, approverId, approverComment);
}
