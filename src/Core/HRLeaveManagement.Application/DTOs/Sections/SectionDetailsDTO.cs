namespace HRLeaveManagement.Application.DTOs.Sections;

public class SectionDetailsDTO
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required string DepartmentName { get; init; }
    public Guid? LeaderId { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime ModifiedAt { get; init; }
}
