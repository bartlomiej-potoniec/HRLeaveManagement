using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Layout.Header;

public partial class HrHeader : ComponentBase
{
    [Parameter, EditorRequired] public required RenderFragment HeaderChildContent { get; set; }
    [Parameter] public RenderFragment? BodyChildContent { get; set; }

    private bool _isInFavourites; 
}
