using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Layout.Dialog;

public partial class HrUpdateUserDialog : HrDialog
{
    [Parameter] public required string UserName { get; set; }
}
