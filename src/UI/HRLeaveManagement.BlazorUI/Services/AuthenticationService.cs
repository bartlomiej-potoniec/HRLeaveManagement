using Blazored.LocalStorage;
using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Models;
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

    public async Task<bool> AuthenticateAsync(string userName, string password)
    {
        try
        {
            var authRequest = new AuthRequest
            {
                UserName = userName,
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

    public async Task<Response<RegistrationResponse>> RegisterAsync(string firstName,
                                                                    string lastName,
                                                                    string email,
                                                                    DateTime dateOfBirth,
                                                                    string? peselNumber,
                                                                    string phoneNumber,
                                                                    List<string> roles)
    {
        Response<RegistrationResponse> response;

        try
        {
            var request = new RegistrationRequest
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                DateOfBirth = dateOfBirth,
                PeselNumber = peselNumber,
                PhoneNumber = phoneNumber,
                Roles = roles
            };

            var data = await _client.RegisterAsync(request);
            response = base.GenerateSuccessResponse("User created successfully", data);
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions<RegistrationResponse>(ex);
        }

        return response;
    }

    public async Task Logout()
        => await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
}
