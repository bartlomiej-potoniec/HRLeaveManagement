using HRLeaveManagement.Application.DTOs.Auth;

namespace HRLeaveManagement.Application.Contracts.Identity;

public interface IAuthService
{
    Task<AuthResponse> Login(AuthRequest request);
    Task<RegistrationResponse> Register(RegistrationRequest request);
    Task ConfirmEmail(string userId, string token);
    Task ChangePassword(PasswordRequest request);
}
