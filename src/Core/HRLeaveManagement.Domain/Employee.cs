using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain;

public class Employee
{
    public Guid Id { get; set; }
    public string Position { get; set; } = default!;
    public string? Responsibilities { get; set; }

    public int SectionId { get; set; }
    public Section Section { get; set; }

    public Guid LeaderId { get; set; }

    public ContractType ContractType { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime? ContractTerminationDate { get; set; }
    public DateTime? DismissalDate { get; set; }

    public EducationType EducationType { get; set; }
    public string? EducationDetails { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}