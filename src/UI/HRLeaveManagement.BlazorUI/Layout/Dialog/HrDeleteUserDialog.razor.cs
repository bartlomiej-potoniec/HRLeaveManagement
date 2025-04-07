using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Layout.Dialog;

public partial class HrDeleteUserDialog : HrDialog
{
    [Parameter] public string? UserName { get; set; }
}
