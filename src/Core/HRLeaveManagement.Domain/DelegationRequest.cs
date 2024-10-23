namespace HRLeaveManagement.Domain;

public class DelegationRequest : WorkRequest
{
    public Guid SubstitutorId { get; set; }
    public string DestinationCountry { get; set; } = default!;
    public string MeansOfTransport { get; set; } = default!;
    public decimal CashAdvance { get; set; }
    public string? PurposeDescription { get; set; }
}