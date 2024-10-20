using Blazored.LocalStorage;
using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Providers;
using HRLeaveManagement.BlazorUI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace HRLeaveManagement.BlazorUI.Services;

public sealed class AuthenticationService(IClient client,
                                          ILocalStorageService localStorage,
                                          AuthenticationStateProvider authenticationStateProvider) 
    : HttpServiceBase(client, localStorage), IAuthenticationService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;

    public async Task<bool> AuthenticateAsync(string email, string password)
    {
        try
        {
            var authRequest = new AuthRequest
            {
                Email = email,
                Password = password
            };

            var authResponse = await _client.LoginAsync(authRequest);

            if (string.IsNullOrEmpty(authResponse.Token)) 
                return false;

            await _localStorage.SetItemAsync("token", authResponse.Token);
            await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedIn();

            return true;
        }

        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> RegisterAsync(string firstName,
                                          string lastName,
                                          string userName,
                                          string email,
                                          string password)
    {
        var registrationRequest = new RegistrationRequest
        {
            FirstName = firstName,
            LastName = lastName,
            UserName = userName,
            Email = email,
            Password = password
        };

        var registrationResponse = await _client.RegisterAsync(registrationRequest);

        return !string.IsNullOrEmpty(registrationResponse.UserId);
    }

    public async Task Logout()
        => await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
}
