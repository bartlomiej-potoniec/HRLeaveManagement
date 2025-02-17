using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Services;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout;

public partial class UserAccountInfoSection
{
    [Inject]
    private ISnackbar Snackbar { get; set; }

    [Inject]
    private IUserService UserService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [CascadingParameter]
    protected Error Error { get; set; }

    [Parameter]
    public UserDetailsViewModel Model { get; set; } = new();

    [Parameter]
    public int Height { get; set; }

    public string? Message { get; set; }

    private async Task LockoutAccount()
    {
        var result = await UserService.LockoutAsync(Model.Id, DateTime.MaxValue);

        Message = result.Message;

        if (!result.IsSuccess)
        {
            Error.HandleError(Message);
            return;
        }

        Snackbar.Add(Message, Severity.Success);
        NavigationManager.NavigateTo($"/users/{Model.Id}/details");
    }

    private async Task UnlockAccount()
    {
        var result = await UserService.UnlockAsync(Model.Id);

        Message = result.Message;

        if (!result.IsSuccess)
        {
            Error.HandleError(Message);
            return;
        }

        Snackbar.Add(Message, Severity.Success);
        NavigationManager.NavigateTo($"/users/{Model.Id}/details");
    }
}
