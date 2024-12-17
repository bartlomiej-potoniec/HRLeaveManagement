namespace HRLeaveManagement.Application.DTOs.Departments;

public record DepartmentDetailsDTO
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required Guid? LeaderId { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime ModifiedAt { get; init; }
}
