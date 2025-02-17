namespace HRLeaveManagement.BlazorUI.ViewModels;

public class RegisterViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? PeselNumber { get; set; }
    public string PhoneNumber { get; set; }
    public List<string> Roles { get; set; } = [];
}
