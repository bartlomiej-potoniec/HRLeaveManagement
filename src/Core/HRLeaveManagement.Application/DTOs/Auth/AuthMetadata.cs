namespace HRLeaveManagement.Application.DTOs.Auth;

public record AuthMetadata
{
    public required string RequestingUserId { get; init; }
    public required string RequestingUserName { get; init; }
    public required string RequestingUserEmail { get; init; }
    public required Guid RequestingEmployeeId { get; init; }
}
