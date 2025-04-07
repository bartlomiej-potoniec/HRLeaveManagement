using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Layout.Section;

public partial class HrLogSection
{
    [Parameter] public string? Height { get; set; } // 350px
    [Parameter] public string? MinHeight { get; set; }
}
