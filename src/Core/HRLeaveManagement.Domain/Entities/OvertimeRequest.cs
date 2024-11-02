namespace HRLeaveManagement.Domain.Entities;

public class OvertimeRequest : WorkRequest
{
    public string? PurposeDescription { get; private set; }

    private OvertimeRequest() {}


    // Factory Methods
    public static OvertimeRequest Create(Guid requestingEmployeeId,
                                         DateOnly startedAt,
                                         DateOnly endedAt,
                                         Guid approverId,
                                         string? approverComment = null,
                                         string? purposeDescription = null)
    {
        var entity = new OvertimeRequest 
        { 
            PurposeDescription = purposeDescription
        };

        entity.InitializeBase(requestingEmployeeId, startedAt, endedAt, approverId, approverComment);

        return entity;
    }
}