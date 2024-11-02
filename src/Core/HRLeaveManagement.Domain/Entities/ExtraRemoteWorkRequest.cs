namespace HRLeaveManagement.Domain.Entities;

public class ExtraRemoteWorkRequest : WorkRequest
{
    public string? ReasonDescription { get; private set; }

    private ExtraRemoteWorkRequest() {}


    // Factory Methods
    public static ExtraRemoteWorkRequest Create(Guid requestingEmployeeId,
                                                DateOnly startedAt,
                                                DateOnly endedAt,
                                                Guid approverId,
                                                string? approverComment = null,
                                                string? reasonDescription = null)
    {
        var entity = new ExtraRemoteWorkRequest
        {
            ReasonDescription = reasonDescription
        };

        entity.InitializeBase(requestingEmployeeId, startedAt, endedAt, approverId, approverComment);

        return entity;
    }
}