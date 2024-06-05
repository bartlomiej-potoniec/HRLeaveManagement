using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages;

public partial class Register
{
    public RegisterViewModel Model { get; set; }

    public string? Message { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    private IAuthenticationService AuthenticationService { get; set; }

    protected override void OnInitialized() => Model = new RegisterViewModel();

    protected async Task HandleRegister()
    {
        var authResult = await AuthenticationService.RegisterAsync(
            Model.FirstName!,
            Model.LastName!,
            Model.UserName!,
            Model.Email!,
            Model.Password!
        );

        if (authResult)
            NavigationManager.NavigateTo("/");

        Message = "Something went wrong, please try again";
    }
}
