namespace HRLeaveManagement.Application.Models.Identity;

public record AuthRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}
