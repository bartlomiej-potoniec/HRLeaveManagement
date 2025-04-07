using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout.Dialog;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout.Menu;

public partial class HrUserAccountDeleteMenuItem : ComponentBase
{
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IUserService UserService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    [CascadingParameter] protected Message Message { get; set; }

    [Parameter, EditorRequired] public string UserId { get; set; }
    [Parameter, EditorRequired] public string UserName { get; set; }

    private async Task DeleteAccountAsync()
    {
        var isValidId = Guid.TryParse(UserId, out Guid id);

        if (!isValidId)
        {
            Message.HandleError("Invalid ID has been provided");
            return;
        }

        var parameters = new DialogParameters
        {
            { "UserName", UserName }
        };

        var dialog = await DialogService.ShowAsync<HrDeleteUserDialog>("User account deleting", parameters);
        var result = await dialog.Result;

        if (result is null || result.Canceled)
        {
            return;
        }

        var response = await UserService.DeleteAsync(id);
        var message = response.Message;

        if (!response.IsSuccess)
        {
            Message.HandleError(message);
            return;
        }

        NavigateToRoot();
        Message.HandleInfo(message);
    }

    private void NavigateToRoot()
    {
        NavigationManager.NavigateTo("/refresh");
        NavigationManager.NavigateTo("/users/list");
    }
}
