using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout.Dialog;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout;

public partial class HrUsersListActionsGroup<T> : ComponentBase where T : class
{
    [Inject] private IUserService UserService { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    [CascadingParameter] protected Message Message { get; set; }
    
    [Parameter, EditorRequired]
    public MudDataGrid<T> DataGrid { get; set; } = new();

    [Parameter] public bool IsChoosen { get; set; } = true;

    private List<Guid> _usersToLock = []; 
    private List<Guid> _usersToUnlock = []; 
    private List<Guid> _usersToDelete = [];

    private bool _isOptionDisabled => DataGrid.SelectedItems.Count == 0;

    private async Task LockoutAccountsAsync()
    {
        var lockoutEnd = DateTime.MaxValue;

        var model = new UserLockoutViewModel();
        var parameters = new DialogParameters
        {
            { "Model", model }
        };

        var dialog = await DialogService.ShowAsync<HrLockoutUserDialog>("User accounts lockout", parameters);
        var result = await dialog.Result;

        if (result is null || result.Canceled)
        {
            return;
        }
        
        _usersToLock = (DataGrid as MudDataGrid<UserDetailsViewModel>)
            .SelectedItems
            .Select(u => u.Id)
            .ToList();

        if (model.LockoutDateEnd.HasValue && model.LockoutTimeEnd.HasValue)
        {
            lockoutEnd = model.LockoutDateEnd.Value + model.LockoutTimeEnd.Value;
        }

        var response = await UserService.LockoutManyAsync(_usersToLock, lockoutEnd);
        var message = response.Message;

        if (!response.IsSuccess)
        {
            Message.HandleError(message);
            return;
        }

        NavigateToRoot();
        Message.HandleInfo(message);
    }

    private async Task UnlockAccountsAsync()
    {
        _usersToUnlock = (DataGrid as MudDataGrid<UserDetailsViewModel>)
            .SelectedItems
            .Select(u => u.Id)
            .ToList();
        
        var response = await UserService.UnlockManyAsync(_usersToUnlock);
        var message = response.Message;

        if (!response.IsSuccess)
        {
            Message.HandleError(message);
            return;
        }

        NavigateToRoot();
        Message.HandleSuccess(message);
    }
    
    private async Task DeleteAccountsAsync()
    {
        var dialog = await DialogService.ShowAsync<HrDeleteUserDialog>("User accounts deleting");
        var result = await dialog.Result;

        if (result is null || result.Canceled)
        {
            return;
        }

        _usersToDelete = (DataGrid as MudDataGrid<UserDetailsViewModel>)
            .SelectedItems
            .Select(u => u.Id)
            .ToList();

        var response = await UserService.DeleteManyAsync(_usersToDelete);
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
