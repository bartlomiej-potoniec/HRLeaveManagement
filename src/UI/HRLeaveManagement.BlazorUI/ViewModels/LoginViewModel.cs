using System.ComponentModel;

namespace HRLeaveManagement.BlazorUI.ViewModels;

public class LoginViewModel
{
    [DisplayName("Username")]
    public string? Username { get; set; }

    [DisplayName("Password")]
    public string? Password { get; set; }
}
