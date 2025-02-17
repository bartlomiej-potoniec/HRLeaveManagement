using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Layout;

public partial class Header
{
    [Parameter] 
    public string Title { get; set; }
    
    [Parameter] 
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string PageUri { get; set; }

    private bool _isInFavourites; 
}
