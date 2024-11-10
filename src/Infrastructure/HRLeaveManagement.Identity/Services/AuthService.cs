using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.DTOs.Identity;
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

public sealed class AuthService(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                ICredentialService credentialService,
                                IIdentityResult identityResult, 
                                IOptions<JwtOptions> jwtOptions)
    : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ICredentialService _credentialService = credentialService;
    private readonly IIdentityResult _identityResult = identityResult;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new NotFoundException($"User with e-mail: { request.Email } not found");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
            throw new BadRequestException(
                _identityResult.ToValidationErrors(result),
                $"Credentials for '{ request.Email }' are not valid"
            );

        var jwtSecurityToken = await GenerateJwtToken(user);
        var token = new JwtSecurityTokenHandler()
            .WriteToken(jwtSecurityToken);

        return new(user.Id, user.UserName!, user.Email!, token);
    }

    public async Task<RegistrationResponse> Register(RegistrationRequest request)
    {
        string userName = _credentialService.GenerateUserLogin(
            request.FirstName,
            request.LastName,
            request.DateOfBirth.ToShortDateString()
        );

        var password = _credentialService.GenerateUserPassword();

        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PeselNumber = request.PeselNumber,
            DateOfBirth = request.DateOfBirth,
            Email = request.Email,
            UserName = userName,
            EmailConfirmed = true // Add logic for email confirmation
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            throw new BadRequestException(_identityResult.ToValidationErrors(result));

        result = await _userManager.AddToRoleAsync(user, "Employee");

        if (!result.Succeeded)
            throw new BadRequestException(_identityResult.ToValidationErrors(result));

        return new(user.Id);
    }

    private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
    {
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

        return jwtSecurityToken;
        /*return new JwtSecurityTokenHandler()
            .WriteToken(jwtSecurityToken);*/
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
