namespace HRLeaveManagement.BlazorUI.ViewModels.Users;

public record UserViewModel
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? PeselNumber { get; init; }
    public required string PhoneNumber { get; init; }
    public required DateTime DateOfBirth { get; init; }
    public Guid? EmployeeId { get; init; }
}
