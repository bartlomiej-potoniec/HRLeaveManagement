using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Layout.Form;

public partial class HrUserSelect<T> where T : class
{
    [Inject] private IUserService UserService { get; set; }

    [CascadingParameter] protected Message Message { get; set; }

    [Parameter, EditorRequired] public required T? User { get; set; }
    [Parameter, EditorRequired] public required Guid? UserId { get; set; }
    [Parameter, EditorRequired] public required Func<T, Guid> IdSelector { get; set; }
    [Parameter, EditorRequired] public required EventCallback<T> UserChanged { get; set; }
    [Parameter, EditorRequired] public required EventCallback<Guid?> UserIdChanged { get; set; }

    private List<UserDetailsViewModel> Users { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        var response = await UserService
            .GetAllAsync(sorts: "EmployeeId", filters: "EmployeeId@=\\null");

        if (!response.IsSuccess)
        {
            Message.HandleError(response.Message);
            return;
        }

        Users = response.Data.Items;
    }

    private async Task SelectedChangedAsync(T? item)
    {
        if (item is null)
        {
            return;
        }

        User = item;
        UserId = IdSelector.Invoke(item);

        await UserChanged.InvokeAsync(User);
        await UserIdChanged.InvokeAsync(UserId);
    }

    private async Task ClearUserClickedAsync()
    {
        User = null;
        UserId = null;

        await UserChanged.InvokeAsync(User);
        await UserIdChanged.InvokeAsync(UserId);
    }
}
