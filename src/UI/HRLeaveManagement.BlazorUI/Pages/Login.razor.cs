using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout;
using HRLeaveManagement.BlazorUI.Validation;
using HRLeaveManagement.BlazorUI.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Pages;

public partial class Login
{
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IAuthenticationService AuthenticationService { get; set; }

    [CascadingParameter] protected Message Message { get; set; }

    private LoginViewModel Model { get; set; } = new();
    private IViewModelValidator<LoginViewModel> Validator => new LoginViewModelValidator();

    private MudForm Form { get; set; }
    private bool _isLoading = false;

    protected async Task HandleLogin()
    {
        await Form.Validate();

        if (!Form.IsValid)
        {
            Message.HandleError(Form.Errors);
            return;
        }

        _isLoading = true;

        var isAuthenticated = await AuthenticationService.AuthenticateAsync(Model.Username, Model.Password);

        if (!isAuthenticated)
        {
            Message.HandleError("Invalid login or password was given");
            _isLoading = false;

            return;
        }

        NavigationManager.NavigateTo("/");

        _isLoading = false;
        StateHasChanged();
    }
}
