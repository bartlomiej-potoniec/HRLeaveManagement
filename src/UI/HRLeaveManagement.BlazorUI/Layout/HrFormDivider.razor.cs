using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Layout;

public partial class HrFormDivider : ComponentBase
{
    [Parameter] public string Text { get; set; }
}
