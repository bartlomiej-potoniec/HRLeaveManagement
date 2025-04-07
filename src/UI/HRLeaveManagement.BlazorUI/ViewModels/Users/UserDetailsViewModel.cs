using System.ComponentModel;

namespace HRLeaveManagement.BlazorUI.ViewModels.Users;

public class UserDetailsViewModel
{
    public Guid Id { get; set; }
    public string ShortId { get; set; }

    [DisplayName("E-mail")]
    public string Email { get; set; }

    [DisplayName("Firstname")]
    public string FirstName { get; set; }

    [DisplayName("Lastname")]
    public string LastName { get; set; }

    [DisplayName("Fullname")]
    public string FullName { get; set; }

    [DisplayName("Username")]
    public string UserName { get; set; }

    [DisplayName("PESEL number")]
    public string? PeselNumber { get; set; }
    public string PeselNumberInfoMessage => PeselNumber is null ? "none" : PeselNumber;
    public bool HasPeselNumber { get; set; }
    public string HasPeselInfoMessage => HasPeselNumber ? "Owns PESEL" : "Does't own PESEL";

    [DisplayName("Phone number")]
    public string? PhoneNumber { get; set; }
    public string PhoneNumberInfoMessage => PhoneNumber is null ? "none" : PhoneNumber;

    [DisplayName("Date of birth")]
    public DateTime? DateOfBirth { get; set; }

    [DisplayName("Account status")]
    public string AccountStatus { get; set; }
    
    public Guid? EmployeeId { get; set; }
    public string EmployeeIdStatus => EmployeeId.HasValue ? EmployeeId.Value.ToString() : "none";
    public string EmployeeAccountStatus => EmployeeId is null ? "Not created" : "Created";

    public bool IsEmailConfirmed { get; set; }
    public string AccountConfirmationStatus => IsEmailConfirmed ? "Confirmed" : "Not confirmed";
    public bool IsLockout { get; set; }
    public string AccountLockoutStatus => IsLockout ? "Blocked" : "Active";
    public DateTime? LockoutEnd { get; set; }
    public string? AccountLockoutDurationStatus => LockoutEnd is null ? "Disabled" : $"until { LockoutEnd.Value }";

    [DisplayName("Roles")]
    public List<string> Roles { get; set; } = [];
}
