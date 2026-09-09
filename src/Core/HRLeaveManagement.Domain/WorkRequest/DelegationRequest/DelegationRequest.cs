namespace HRLeaveManagement.Domain.WorkRequest.DelegationRequest;

public class DelegationRequest : WorkRequest
{
    public Guid SubstitutorId { get; private set; }
    public Employee.Employee Substitutor { get; private set; }

    public string DestinationCountry { get; private set; }
    public string MeansOfTransport { get; private set; }
    public decimal CashAdvance { get; private set; }
    public string? PurposeDescription { get; private set; }

    private DelegationRequest() {}

    #region Domain_Factory_Methods

    /// <summary>
    /// Creates <see cref="DelegationRequest"/> instance for given params.
    /// Designates the only way to properly create an object.
    /// </summary>
    /// <param name="workRequestRuleSet">Instance of <see cref="IWorkRequestRuleSet"/> for rules checking</param>
    /// <param name="requestingEmployee">Employee requesting delegation</param>
    /// <param name="startedAt">Date of delegation starting</param>
    /// <param name="endedAt">Date of delegation ending</param>
    /// <param name="approver">Superior approving request</param>
    /// <param name="substitutor">Employee substituting absent Employee</param>
    /// <param name="destinationCountry">Delegation destination country</param>
    /// <param name="meansOfTransport">Means of transport while delegation</param>
    /// <param name="cashAdvance">Delegation cash advance for employee</param>
    /// <param name="approverComment">Optional. Comment of superior approving request</param>
    /// <param name="purposeDescription">Optional. Purpose of delegation</param>
    /// <param name="cancellationToken">Cancellation Token for operation breaking</param>
    /// <returns>A new instance of <see cref="DelegationRequest"/></returns>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public static async Task<DelegationRequest> Create(IWorkRequestRuleSet workRequestRuleSet,
                                                       Employee requestingEmployee,
                                                       DateOnly startedAt,
                                                       DateOnly endedAt,
                                                       Employee approver,
                                                       Employee substitutor,
                                                       string destinationCountry,
                                                       string meansOfTransport,
                                                       decimal cashAdvance,
                                                       string? approverComment = null,
                                                       string? purposeDescription = null,
                                                       CancellationToken cancellationToken = default)
    {
        ValidateBaseRules(requestingEmployee, substitutor, cashAdvance);

        var delegationRequest = new DelegationRequest()
        {
            Substitutor = substitutor,
            DestinationCountry = destinationCountry,
            MeansOfTransport = meansOfTransport,
            CashAdvance = cashAdvance,
            PurposeDescription = purposeDescription
        };

        await delegationRequest.InitializeBase<DelegationRequest>(
            requestingEmployee,
            startedAt,
            endedAt,
            approver,
            approverComment,
            workRequestRuleSet,
            cancellationToken
        );
        
        return delegationRequest;
    }

    private static void ValidateBaseRules(Employee requestingEmployee,
                                          Employee substitutor,
                                          decimal cashAdvance)
    {
        if (requestingEmployee == substitutor)
        {
            throw new ArgumentException("Requesting employee cannot be their own substitutor");
        }

        if (cashAdvance < 0)
        {
            throw new ArgumentException("Cash advance must be greater than 0");
        }
    }

    #endregion
}