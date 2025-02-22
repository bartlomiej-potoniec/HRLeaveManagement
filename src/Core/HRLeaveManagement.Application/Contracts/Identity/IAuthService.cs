using HRLeaveManagement.Application.DTOs.Auth;

namespace HRLeaveManagement.Application.Contracts.Identity;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(AuthRequest request, CancellationToken cancellationToken);
    Task<RegistrationResponse> RegisterAsync(RegistrationRequest request, CancellationToken cancellationToken);
    Task ConfirmEmailAsync(string userId, string token, CancellationToken cancellationToken);
    Task ChangePasswordAsync(PasswordRequest request, CancellationToken cancellationToken);
}
