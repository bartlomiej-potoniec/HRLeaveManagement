using HRLeaveManagement.Domain.RuleContracts;

namespace HRLeaveManagement.Domain.Entities;

public class ExtraRemoteWorkRequest : WorkRequest
{
    public string? ReasonDescription { get; private set; }

    private ExtraRemoteWorkRequest() {}


    #region Domain_Factory_Methods

    /// <summary>
    /// Creates <see cref="ExtraRemoteWorkRequest"/> instance for given params.
    /// Designates the only way to properly create an object.
    /// </summary>
    /// <param name="requestingEmployee">Employee requesting remote work</param>
    /// <param name="startedAt">Date of remote working starting</param>
    /// <param name="endedAt">Date of remote working ending</param>
    /// <param name="approver">Superior approving request</param>
    /// <param name="approverComment">Comment of superior approving request</param>
    /// <param name="reasonDescription">Reason of extra remote work</param>
    /// <param name="workRequestRuleSet">Instance of <see cref="IWorkRequestRuleSet"/> for rules checking</param>
    /// <param name="cancellationToken">Cancellation Token for operation breaking</param>
    /// <returns>A new instance of <see cref="ExtraRemoteWorkRequest"/></returns>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public static async Task<ExtraRemoteWorkRequest> Create(IWorkRequestRuleSet workRequestRuleSet,
                                                            Employee requestingEmployee,
                                                            DateOnly startedAt,
                                                            DateOnly endedAt,
                                                            Employee approver,
                                                            string? approverComment = null,
                                                            string? reasonDescription = null,
                                                            CancellationToken cancellationToken = default)
    {
        var entity = new ExtraRemoteWorkRequest
        {
            ReasonDescription = reasonDescription
        };

        await entity.InitializeBase<ExtraRemoteWorkRequest>(
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