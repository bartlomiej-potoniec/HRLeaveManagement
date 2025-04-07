using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Layout.Section;

public partial class HrSection : ComponentBase
{
    [Parameter] public RenderFragment? HeaderChildContent { get; set; }
    [Parameter] public RenderFragment? BodyChildContent { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public string Icon { get; set; }
    [Parameter] public string? Height { get; set; } //590px;
    [Parameter] public string? MinHeight { get; set; }
    [Parameter] public bool HasOverflowScroll { get; set; } = false;
    [Parameter] public bool HasLoader { get; set; } = false;
    [Parameter] public bool IsLoading { get; set; } = false;

    protected override void OnParametersSet()
    {
        if (!HasLoader && IsLoading)
        {
            throw new InvalidOperationException("Parameter 'IsLoading' can be set only if 'HasValue' parameter is set on true");
        }

        if (!string.IsNullOrEmpty(Height) && !string.IsNullOrEmpty(MinHeight))
        {
            throw new InvalidOperationException("It is forbidden to set both 'Height' & 'MinHeight' at the same time");
        }
    }
}
