using HRLeaveManagement.BlazorUI.Models;
using HRLeaveManagement.BlazorUI.Services.Base;

namespace HRLeaveManagement.BlazorUI.Contracts;

public interface IAuthenticationService
{
    Task<bool> AuthenticateAsync(string email, string password);
    Task<Response<RegistrationResponse>> RegisterAsync(string firstName,
                                                       string lastName,
                                                       string email,
                                                       DateTime dateOfBirth,
                                                       string? peselNumber,
                                                       string phoneNumber,
                                                       List<string> roles);
    Task Logout();
}
