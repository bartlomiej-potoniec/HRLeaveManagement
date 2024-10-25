using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Entities;

public class DelegationRequest : WorkRequest
{
    public Guid SubstitutorId { get; private set; }
    public string DestinationCountry { get; private set; }
    public string MeansOfTransport { get; private set; }
    public decimal CashAdvance { get; private set; }
    public string? PurposeDescription { get; private set; }

    private DelegationRequest() { }


    // Factory Methods
    public static DelegationRequest Create(Guid employeeId,
                                           DateTime startedAt,
                                           DateTime endedAt,
                                           Guid approverId,
                                           string? approverComment,
                                           Guid substitutorId,
                                           string destinationCountry,
                                           string meansOfTransport,
                                           decimal cashAdvance,
                                           string? purposeDescription)
        => new()
        {
            EmployeeId = employeeId,
            StartedAt = startedAt,
            EndedAt = endedAt,
            TotalDays = (int)(endedAt - startedAt).TotalDays,
            ApproverId = approverId,
            ApproverComment = approverComment,
            Status = RequestStatus.Pending,
            SubstitutorId = substitutorId,
            DestinationCountry = destinationCountry,
            MeansOfTransport = meansOfTransport,
            CashAdvance = cashAdvance,
            PurposeDescription = purposeDescription,
            CreatedAt = DateTime.UtcNow
        };
}