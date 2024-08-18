using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout;
using HRLeaveManagement.BlazorUI.ViewModels;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages;

public partial class Login
{
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    private IAuthenticationService AuthenticationService { get; set; }

    [CascadingParameter]
    protected Error Error { get; set; } 

    public required LoginViewModel Model { get; set; }

    public string? Message { get; set; }

    protected override void OnInitialized() => Model = new();

    protected async Task HandleLogin()
    {
        var authResult = await AuthenticationService
            .AuthenticateAsync(Model.Email, Model.Password);

        if (authResult)
        {
            NavigationManager.NavigateTo("/");
            return;
        }

        Message = "Nieprawidłowy login lub hasło";
        Error.HandleError(Message);
    }
}
