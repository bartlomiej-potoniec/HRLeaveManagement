namespace HRLeaveManagement.Application.DTOs.LeaveTypes;

public record LeaveTypeDTO
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required decimal PaidFraction { get; init; }
}