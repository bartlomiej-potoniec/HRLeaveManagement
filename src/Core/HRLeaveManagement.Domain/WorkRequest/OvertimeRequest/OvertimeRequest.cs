namespace HRLeaveManagement.Domain.WorkRequest.OvertimeRequest;

public class OvertimeRequest : WorkRequest
{
    public string? PurposeDescription { get; private set; }

    private OvertimeRequest() {}

    #region Domain_Factory_Methods

    /// <summary>
    /// Creates <see cref="OvertimeRequest"/> instance for given params.
    /// Designates the only way to properly create an object.
    /// </summary>
    /// <param name="requestingEmployee">Employee requesting overtime</param>
    /// <param name="startedAt">Date of overtime starting</param>
    /// <param name="endedAt">Date of overtime ending</param>
    /// <param name="approver">Superior approving request</param>
    /// <param name="approverComment">Comment of superior approving request</param>
    /// <param name="purposeDescription">Purpose of overtime</param>
    /// <param name="workRequestRuleSet">Instance of <see cref="IWorkRequestRuleSet"/> for rules checking</param>
    /// <param name="cancellationToken">Cancellation Token for operation breaking</param>
    /// <returns>A new instance of <see cref="OvertimeRequest"/></returns>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public static async Task<OvertimeRequest> Create(IWorkRequestRuleSet workRequestRuleSet, Employee requestingEmployee,
                                                     DateOnly startedAt,
                                                     DateOnly endedAt,
                                                     Employee approver,
                                                     string? approverComment = null,
                                                     string? purposeDescription = null,
                                                     CancellationToken cancellationToken = default)
    {
        var entity = new OvertimeRequest 
        { 
            PurposeDescription = purposeDescription
        };

        await entity.InitializeBase<OvertimeRequest>(
            requestingEmployee,
            startedAt,
            endedAt,
            approver,
            approverComment,
            workRequestRuleSet,
            cancellationToken
        );

        return entity;
    }

    #endregion
}
