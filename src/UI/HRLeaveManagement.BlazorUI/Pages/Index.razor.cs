using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace HRLeaveManagement.BlazorUI.Pages;

public partial class Index
{
    [Inject] private IAuthenticationService AuthenticationService { get; set; }
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
        => await ((ApiAuthenticationStateProvider)AuthenticationStateProvider)
            .GetAuthenticationStateAsync();

    private async void Logout() => await AuthenticationService.Logout();
}
