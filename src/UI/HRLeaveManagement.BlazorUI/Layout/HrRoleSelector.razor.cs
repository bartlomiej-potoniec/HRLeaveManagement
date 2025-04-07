using HRLeaveManagement.BlazorUI.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace HRLeaveManagement.BlazorUI.Layout;

public partial class HrRoleSelector : ComponentBase
{
    [Inject] public IRoleService RoleService { get; set; }

    [CascadingParameter] protected Message Message { get; set; }

    [Parameter] public required List<string> Roles { get; set; } = [];
    [Parameter] public EventCallback<List<string>> RolesChanged { get; set; }

    private List<string> AvailableRoles { get; set; } = [];
    private string? SelectedRole { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var response = await RoleService.GetAllAsync();

        if (!response.IsSuccess)
        {
            Message.HandleError("No roles to display");
            return;
        }

        AvailableRoles = response.Data
            .Select(role => role.Name)
            .ToList();
    }

    private bool IsAddRoleButtonDisabled => string.IsNullOrWhiteSpace(SelectedRole) || Roles.Contains(SelectedRole);

    private async Task AddRoleAsync(string role)
    {
        if (!Roles.Contains(role) && AvailableRoles.Contains(role))
        {
            Roles.Add(role);
            await RolesChanged.InvokeAsync(Roles);
        }
    }

    private async Task AddRoleFromButtonAsync()
    {
        if (!string.IsNullOrWhiteSpace(SelectedRole))
        {
            await AddRoleAsync(SelectedRole);
            SelectedRole = null;
        }
    }

    private async Task HandleKeyDownAsync(KeyboardEventArgs e)
    {
        if (e.Key == "Enter" && !string.IsNullOrEmpty(SelectedRole))
        {
            await AddRoleAsync(SelectedRole);
            SelectedRole = null;
        }
    }

    private Task<IEnumerable<string>> SearchRolesAsync(string value, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Task.FromResult(AvailableRoles.Except(Roles).AsEnumerable());

        return Task.FromResult(AvailableRoles
            .Except(Roles)
            .Where(r => r.Contains(value, StringComparison.InvariantCultureIgnoreCase))
            .AsEnumerable()
        );
    }
}
