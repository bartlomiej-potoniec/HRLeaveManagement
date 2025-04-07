using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout.Dialog;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout.Table;

public partial class HrUserActionsCell : ComponentBase
{
    [Inject] private IUserService UserService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IDialogService DialogService { get; set; }

    [CascadingParameter] protected Message Message { get; set; }

    [Parameter, EditorRequired] public required string UserId { get; set; }
    [Parameter, EditorRequired] public required string UserName { get; set; }
    [Parameter, EditorRequired] public required bool IsUserLockout { get; set; }

    private async Task DeleteAsync(string userId)
    {
        var isValidId = Guid.TryParse(userId, out Guid id);

        if (!isValidId)
        {
            Message.HandleError("Invalid ID has been provided");
            return;
        }

        var parameters = new DialogParameters
        {
            { "UserName", UserName }
        };

        var dialog = await DialogService.ShowAsync<HrDeleteUserDialog>("User deleting", parameters);
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

        NavigationManager.NavigateTo("/refresh");
        NavigationManager.NavigateTo("/users/list");
        Message.HandleInfo(message);
    }
}
