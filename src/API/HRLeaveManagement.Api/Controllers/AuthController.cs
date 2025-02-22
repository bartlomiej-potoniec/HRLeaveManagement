using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRLeaveManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request,
                                                        CancellationToken cancellationToken)
    {
        var response = await _authService.LoginAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("register")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<RegistrationResponse>> Register([FromBody] RegistrationRequest request,
                                                                   CancellationToken cancellationToken)
    {
        var response = await _authService.RegisterAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpGet("email-confirmation")]
    public async Task<ActionResult> ConfirmEmail([FromQuery] string userId,
                                                 [FromQuery] string token,
                                                 CancellationToken cancellationToken)
    {
        await _authService.ConfirmEmailAsync(userId, token, cancellationToken);
        return Ok();
    }

    [HttpPost("password-reset")]
    [Authorize(Roles = "Administrator, Employee")]
    public async Task<ActionResult> ChangePassword([FromBody] PasswordRequest request,
                                                   CancellationToken cancellationToken)
    {
        await _authService.ChangePasswordAsync(request, cancellationToken);
        return Ok();
    }
}
