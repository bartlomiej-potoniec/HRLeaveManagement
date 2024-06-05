using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace HRLeaveManagement.BlazorUI.Pages;

public partial class Index
{
    [Inject]
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public IAuthenticationService AuthenticationService { get; set; }

    protected override async Task OnInitializedAsync()
        => await ((ApiAuthenticationStateProvider)AuthenticationStateProvider)
            .GetAuthenticationStateAsync();

    protected void GoToLogin() => NavigationManager.NavigateTo("login/");
    protected void GoToRegister() => NavigationManager.NavigateTo("register/");

    protected async void Logout() => await AuthenticationService.Logout();
}
