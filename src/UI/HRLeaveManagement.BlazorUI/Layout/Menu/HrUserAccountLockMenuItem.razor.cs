using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout.Dialog;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout.Menu;

public partial class HrUserAccountLockMenuItem : ComponentBase
{
    [Inject] private IUserService UserService { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    [CascadingParameter] protected Message Message { get; set; }

    [Parameter, EditorRequired] public required string UserId { get; set; }
    [Parameter, EditorRequired] public required string UserName { get; set; }
    [Parameter, EditorRequired] public required bool IsUserLockout { get; set; }

    private Guid _userId;

    protected override void OnParametersSet()
    {
        var isValidId = Guid.TryParse(UserId, out Guid id);

        if (!isValidId)
        {
            Message.HandleError("Invalid ID has been provided");
            return;
        }

        _userId = id;
        StateHasChanged();
    }

    private async Task LockoutAccountAsync()
    {
        var lockoutEnd = DateTime.MaxValue;

        var model = new UserLockoutViewModel();
        var parameters = new DialogParameters
        {
            { "Model", model },
            { "UserName", UserName }
        };

        var dialog = await DialogService.ShowAsync<HrLockoutUserDialog>("User account lockout", parameters);
        var result = await dialog.Result;

        if (result is null || result.Canceled)
        {
            return;
        }

        if (model.LockoutDateEnd.HasValue && model.LockoutTimeEnd.HasValue)
        {
            lockoutEnd = model.LockoutDateEnd.Value + model.LockoutTimeEnd.Value;
        }

        var response = await UserService.LockoutAsync(_userId, DateTime.MaxValue);
        var message = response.Message;

        if (!response.IsSuccess)
        {
            Message.HandleError(message);
            return;
        }

        NavigateToRoot();
        Message.HandleInfo(message);
    }

    private async Task UnlockAccountAsync()
    {
        var response = await UserService.UnlockAsync(_userId);
        var message = response.Message;

        if (!response.IsSuccess)
        {
            Message.HandleError(message);
            return;
        }

        NavigateToRoot();
        Message.HandleSuccess(message);
    }

    private void NavigateToRoot()
    {
        NavigationManager.NavigateTo($"/refresh");
        NavigationManager.NavigateTo($"/users/{_userId}/details");
    }
}
