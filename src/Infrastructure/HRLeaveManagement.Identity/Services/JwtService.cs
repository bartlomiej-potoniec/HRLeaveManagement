using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Identity.Models;
using HRLeaveManagement.Identity.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRLeaveManagement.Identity.Services;

public sealed class JwtService(UserManager<ApplicationUser> userManager,
                               IOptions<JwtOptions> jwtOptions) 
    : IJwtService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<string> GenerateJwtToken(string userName)
    {
        var user = await _userManager
            .FindByNameAsync(userName)
            ?? throw new NotFoundException($"User with username: { userName } not found");

        var claims = await GetUserClaims(user);

        var symmetricSecurityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtOptions.Key)
        );

        var signingCredentials = new SigningCredentials(
            symmetricSecurityKey,
            SecurityAlgorithms.HmacSha256
        );

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.DurationInMinutes),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(jwtSecurityToken);
    }

    private async Task<IEnumerable<Claim>> GetUserClaims(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var userRoles = await _userManager.GetRolesAsync(user);

        var roleClaims = userRoles
            .Select(role => new Claim(ClaimTypes.Role, role))
            .ToList();

        Claim[] initialClaims = [
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName!),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new("uid", user.Id)
        ];

        var claims = initialClaims
            .Union(userClaims)
            .Union(roleClaims);

        return claims;
    }
}
