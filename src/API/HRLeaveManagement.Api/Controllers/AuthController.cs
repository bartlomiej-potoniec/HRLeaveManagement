using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace HRLeaveManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
    {
        var response = await _authService.Login(request);
        return Ok(response);
    }

    [HttpPost("register")]
    //[Authorize(Roles = "Administrator")]
    public async Task<ActionResult<RegistrationResponse>> Register([FromBody] RegistrationRequest request)
    {
        var response = await _authService.Register(request);
        return Ok(response);
    }

    [HttpGet("email-confirmation")]
    public async Task<ActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        await _authService.ConfirmEmail(userId, token);
        return Ok();
    }

    [HttpPost("password-reset")]
    //[Authorize(Roles = "Administrator, Employee")]
    public async Task<ActionResult> ChangePassword([FromBody] PasswordRequest request)
    {
        await _authService.ChangePassword(request);
        return Ok();
    }
}
