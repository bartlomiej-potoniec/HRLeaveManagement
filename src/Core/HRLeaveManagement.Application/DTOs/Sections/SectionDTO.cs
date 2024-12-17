namespace HRLeaveManagement.Application.DTOs.Sections;

public record SectionDTO
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required string DepartmentName { get; init; }
    public required Guid? LeaderId { get; init; }
}
