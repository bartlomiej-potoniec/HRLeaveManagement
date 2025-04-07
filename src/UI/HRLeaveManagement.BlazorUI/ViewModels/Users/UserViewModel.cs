using System.ComponentModel;

namespace HRLeaveManagement.BlazorUI.ViewModels.Users;

public class UserViewModel
{
    public required Guid Id { get; set; }
    public Guid? EmployeeId { get; set; }

    [DisplayName("E-mail")]
    public required string Email { get; set; }

    [DisplayName("Firstname")]
    public required string FirstName { get; set; }

    [DisplayName("Lastname")]
    public required string LastName { get; set; }

    [DisplayName("PESEL number")]
    public string? PeselNumber { get; set; }

    [DisplayName("Phone number")]
    public required string PhoneNumber { get; set; }

    [DisplayName("Date of birth")]
    public required DateTime DateOfBirth { get; set; }
}
