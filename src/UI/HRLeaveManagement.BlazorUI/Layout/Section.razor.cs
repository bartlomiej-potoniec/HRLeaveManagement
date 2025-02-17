using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout;

public partial class Section
{
    [Parameter]
    public string Title { get; set; }

    [Parameter]
    public string Icon { get; set; }

    [Parameter]
    public RenderFragment? HeaderChildContent { get; set; }

    [Parameter]
    public RenderFragment? BodyChildContent { get; set; }

    [Parameter]
    public int Height { get; set; }

    [Parameter]
    public object Model { get; set; }

    [Parameter]
    public Func<Task> Validation { get; set; }

    [Parameter]
    public MudForm Ref { get; set; }
}
