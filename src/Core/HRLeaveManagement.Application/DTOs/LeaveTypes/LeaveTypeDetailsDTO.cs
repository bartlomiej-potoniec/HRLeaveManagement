namespace HRLeaveManagement.Application.DTOs.LeaveTypes;

public record LeaveTypeDetailsDTO
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required decimal PaidFraction { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime ModifiedAt { get; init; }
}
