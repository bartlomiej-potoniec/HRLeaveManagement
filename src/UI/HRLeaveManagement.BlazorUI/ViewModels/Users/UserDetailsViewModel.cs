namespace HRLeaveManagement.BlazorUI.ViewModels.Users;

public class UserDetailsViewModel
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string? PeselNumber { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Guid? EmployeeId { get; set; }

    public bool IsEmailConfirmed { get; set; }
    public bool IsLockout { get; set; }
    public DateTime? LockoutEnd { get; set; }

    public List<string> Roles { get; set; } = [];

    public string ShortId { get; set; }
    public string FullName { get; set; }
    public string RolesAsString { get; set; }
    public string AccountStatus { get; set; }
}
