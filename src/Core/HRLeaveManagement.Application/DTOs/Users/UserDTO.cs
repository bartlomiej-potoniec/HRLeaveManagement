namespace HRLeaveManagement.Application.DTOs.Users;

public record UserDTO
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? PeselNumber { get; init; }
    public required string PhoneNumber { get; init; }
    public required DateTime DateOfBirth { get; init; }
    public required Guid? EmployeeId { get; init; }
}
