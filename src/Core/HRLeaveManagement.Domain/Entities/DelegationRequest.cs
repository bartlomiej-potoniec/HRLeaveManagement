namespace HRLeaveManagement.Domain.Entities;

public class DelegationRequest : WorkRequest
{
    public Guid SubstitutorId { get; private set; }
    public Employee Substitutor { get; private set; }

    public string DestinationCountry { get; private set; }
    public string MeansOfTransport { get; private set; }
    public decimal CashAdvance { get; private set; }
    public string? PurposeDescription { get; private set; }

    private DelegationRequest() {}


    #region Domain_Factory_Methods

    public static DelegationRequest Create(Guid requestingEmployeeId,
                                           DateOnly startedAt,
                                           DateOnly endedAt,
                                           Guid approverId,
                                           Guid substitutorId,
                                           string destinationCountry,
                                           string meansOfTransport,
                                           decimal cashAdvance,
                                           string? approverComment = null,
                                           string? purposeDescription = null)
    {
        var entity = new DelegationRequest()
        {
            SubstitutorId = substitutorId,
            DestinationCountry = destinationCountry,
            MeansOfTransport = meansOfTransport,
            CashAdvance = cashAdvance,
            PurposeDescription = purposeDescription
        };

        entity.InitializeBase(requestingEmployeeId, startedAt, endedAt, approverId, approverComment);

        return entity;
    }

    #endregion
}