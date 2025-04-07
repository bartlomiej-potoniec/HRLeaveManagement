using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Layout.Progress;

public partial class HrProgressCircular : ComponentBase
{
    [Parameter] public bool DataLoaded { get; set; }
}
