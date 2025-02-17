using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Layout;

public partial class UserInfoSection
{
    [Parameter]
    public UserDetailsViewModel? Model { get; set; }

    [Parameter]
    public int Height { get; set; }


}
