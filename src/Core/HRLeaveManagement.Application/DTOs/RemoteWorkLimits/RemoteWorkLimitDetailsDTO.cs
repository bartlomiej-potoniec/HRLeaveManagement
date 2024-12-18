namespace HRLeaveManagement.Application.DTOs.RemoteWorkLimits;

public record RemoteWorkLimitDetailsDTO
{
    public required int Id { get; init; }
    public required Guid EmployeeId { get; init; }
    public required int Year { get; init; }
    public required int AvailableDays { get; init; }
    public required int UsedDays { get; init; }
    public required int RemainingDays { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime ModifiedAt { get; init; }
}
