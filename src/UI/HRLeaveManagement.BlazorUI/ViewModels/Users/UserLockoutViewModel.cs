using System.ComponentModel;

namespace HRLeaveManagement.BlazorUI.ViewModels.Users;

public class UserLockoutViewModel
{
    [DisplayName("Lockout end date")]
    public DateTime? LockoutDateEnd { get; set; }

    [DisplayName("Lockout time date")]
    public TimeSpan? LockoutTimeEnd { get; set; }
}
