namespace HRLeaveManagement.Application.DTOs.Departments;

public record DepartmentDTO
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required Guid? LeaderId { get; init; }
}
