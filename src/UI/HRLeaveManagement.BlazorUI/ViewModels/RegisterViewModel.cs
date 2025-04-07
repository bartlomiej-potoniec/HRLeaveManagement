using System.ComponentModel;

namespace HRLeaveManagement.BlazorUI.ViewModels;

public class RegisterViewModel
{
    [DisplayName("Firstname")]
    public string? FirstName { get; set; }

    [DisplayName("Lastname")]
    public string? LastName { get; set; }

    [DisplayName("E-mail")]
    public string? Email { get; set; }

    [DisplayName("Date of birth")]
    public DateTime? DateOfBirth { get; set; }

    [DisplayName("PESEL Number")]
    public string? PeselNumber { get; set; }
    public bool HasPeselNumber { get; set; }
    public string HasPeselInfoMessage => HasPeselNumber ? "Owns PESEL" : "Doesn't own PESEL";

    [DisplayName("Phone number")]
    public string? PhoneNumber { get; set; }

    [DisplayName("Roles")]
    public List<string> Roles { get; set; } = [];
}
