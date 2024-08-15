using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Models.Identity;
using HRLeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRLeaveManagement.Identity.Services;

public sealed class AuthService(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                IOptions<JwtSettings> jwtSettings) 
    : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new NotFoundException($"User with e-mail: { request.Email } not found");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
            throw new BadRequestException($"Credentials for '{ request.Email }' are not valid");

        var jwtSecurityToken = await GenerateJwtToken(user);
        var token = new JwtSecurityTokenHandler()
            .WriteToken(jwtSecurityToken);

        return new(user.Id, user.UserName!, user.Email!, token);
    }

    public async Task<RegistrationResponse> Register(RegistrationRequest request)
    {
        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.UserName,
            FirstName = request.FirstName,
            LastName = request.LastName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new BadRequestException($"{ result.Errors }");

        await _userManager.AddToRoleAsync(user, "Employee");

        return new(user.Id);
    }

    private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
    {
        var claims = await GetUserClaims(user);

        var symmetricSecurityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Key)
        );

        var signingCredentials = new SigningCredentials(
            symmetricSecurityKey, 
            SecurityAlgorithms.HmacSha256
        );

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials
        );

        return jwtSecurityToken;
    }

    private async Task<IEnumerable<Claim>> GetUserClaims(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var userRoles = await _userManager.GetRolesAsync(user);

        var roleClaims = userRoles
            .Select(role => new Claim(ClaimTypes.Role, role))
            .ToList();

        var initialClaims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, user.UserName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new("uid", user.Id)
        };

        var claims = initialClaims
            .Union(userClaims)
            .Union(roleClaims);

        return claims;
    }
}
