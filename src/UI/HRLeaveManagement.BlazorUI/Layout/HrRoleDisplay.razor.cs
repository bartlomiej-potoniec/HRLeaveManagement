using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout;

public partial class HrRoleDisplay : ComponentBase
{
    [Parameter] public required List<string> Roles { get; set; }
    [Parameter] public EventCallback<List<string>> RolesChanged { get; set; }
    [Parameter] public string? ForId { get; set; }
    [Parameter] public bool IsReadOnly { get; set; } = false;

    [Parameter] public Color ChipColor { get; set; } = Color.Default;
    [Parameter] public Size ChipSize { get; set; } = Size.Medium;

    [Parameter] public string MarginTop { get; set; } = "0";
    [Parameter] public string MaxWrapWidth { get; set; } = "100%";
    [Parameter] public string MinLabelWidth { get; set; } = "230px";

    private async Task RemoveRoleAsync(string role)
    {
        if (!IsReadOnly)
        {
            Roles.Remove(role);
            await RolesChanged.InvokeAsync(Roles);
        }
    }
}
