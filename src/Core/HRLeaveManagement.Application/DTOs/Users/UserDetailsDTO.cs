namespace HRLeaveManagement.Application.DTOs.Users;

public record UserDetailsDTO
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string FullName { get; init; }
    public required string UserName { get; init; }
    public string? PeselNumber { get; init; }
    public required string PhoneNumber { get; init; }
    public required DateTime DateOfBirth { get; init; }
    public Guid? EmployeeId { get; init; }

    public required bool IsEmailConfirmed { get; init; }
    public required bool IsLockout { get; init; }
    public DateTime? LockoutEnd { get; init; }
    public required string AccountStatus { get; init; }

    public required List<string> Roles { get; init; }
}