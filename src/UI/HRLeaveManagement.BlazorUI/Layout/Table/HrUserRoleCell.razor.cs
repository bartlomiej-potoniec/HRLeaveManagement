using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Layout.Table;

public partial class HrUserRoleCell : ComponentBase
{
    [Parameter, EditorRequired]
    public required List<string> Roles { get; set; }
}
