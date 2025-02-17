using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.DTOs.Auth;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Identity.DbContexts;
using HRLeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace HRLeaveManagement.Identity.Services;

public sealed class AuthService(SignInManager<ApplicationUser> signInManager,
                                IServiceProvider serviceProvider,
                                ICredentialService credentialService,
                                IJwtService jwtService,
                                IEmailService emailService,
                                IUserService userService,
                                IIdentityResult identityResult,
                                IAppLogger<AuthService> logger)
    : IAuthService
{
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ICredentialService _credentialService = credentialService;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IEmailService _emailService = emailService;
    private readonly IUserService _userService = userService;
    private readonly IIdentityResult _identityResult = identityResult;
    private readonly IAppLogger<AuthService> _logger = logger;

    private readonly ApplicationIdentityDbContext _dbContext
        = serviceProvider.GetRequiredService<ApplicationIdentityDbContext>();

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        var user = await _signInManager.UserManager
            .FindByNameAsync(request.UserName)
            ?? throw new NotFoundException($"User with username: { request.UserName } not found");

        _logger.LogInformation("Logging in started for user {Username} with id: {Id}", user.UserName!, user.Id);

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            _logger.LogError("Checking password failed for user {Username} with id: {Id}", user.UserName!, user.Id);
            throw new BadRequestException($"Credentials for '{ request.UserName }' are not valid");
        }
            
        var jwtSecurityToken = await _jwtService.GenerateJwtToken(user.UserName!);

        _logger.LogInformation("Logging in successful for user {Username} with id: {Id}", user.UserName!, user.Id);

        return new(user.Id, user.UserName!, user.Email!, jwtSecurityToken);
    }

    public async Task<RegistrationResponse> Register(RegistrationRequest request)
    {
        string userName = _credentialService.GenerateUserLogin(
            request.FirstName,
            request.LastName,
            request.DateOfBirth.ToString("yyyyMMdd")
        );

        var password = _credentialService.GenerateUserPassword();

        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PeselNumber = request.PeselNumber,
            PhoneNumber = request.PhoneNumber,
            DateOfBirth = DateOnly.FromDateTime(request.DateOfBirth),
            Email = request.Email,
            UserName = userName,
            EmailConfirmed = false
        };

        _logger.LogInformation("Starting transaction for registering user {Username}", userName);
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            _logger.LogInformation("Creating account started for user {Username}", userName);
            
            var result = await _signInManager.UserManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                _logger.LogError("Creating account failed for user {Username}", userName);
                throw new BadRequestException(
                    _identityResult.ToValidationErrors(result),
                    "Cannot create a new user for given credentials"
                );
            }

            _logger.LogInformation("Creating account successful for user {Username}", userName);
            _logger.LogInformation("Adding to roles {Roles} started for user {Username}", request.Roles, userName);

            result = await _signInManager.UserManager.AddToRolesAsync(user, request.Roles);

            if (!result.Succeeded)
            {
                _logger.LogError("Adding to roles {Roles} failed for user {Username} ", request.Roles, userName);
                throw new BadRequestException(
                    _identityResult.ToValidationErrors(result),
                    $"Cannot add a new user to roles '{ request.Roles }'"
                );
            }

            _logger.LogInformation("Adding to roles {Roles} successful for user {Username} ", request.Roles, userName);

            var token = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = _emailService.GenerateEmailConfirmationLink(user.Id, token);

            await _emailService.SendRegistrationEmail(
                request.Email,
                request.FirstName,
                userName,
                password,
                confirmationLink
            );

            await transaction.CommitAsync();
            _logger.LogInformation("Transaction successful for registering user {Username}", userName);

            return new(user.Id);
        }

        catch (Exception ex)
        {
            _logger.LogError("Transaction failed for registering user {Username}", userName);
            await transaction.RollbackAsync();

            throw;
        }
    }

    public async Task ConfirmEmail(string? userId, string? token)
    {
        if (userId is null or "" || token is null or "")
            throw new BadRequestException("Invalid user ID or token");
        
        var user = await _signInManager.UserManager
            .FindByIdAsync(userId)
            ?? throw new NotFoundException($"No user with ID: { userId } found");

        _logger.LogInformation("Confirming email started for user {Username} with ID: {Id}", user.UserName!, userId);

        var isEmailConfirmed = await _signInManager.UserManager.IsEmailConfirmedAsync(user);
        
        if (isEmailConfirmed)
        {
            _logger.LogError("Confirming email failed for user {Username} with ID: {Id}", user.UserName!, userId);
            throw new BadRequestException($"Email for { user.Email } is already confirmed");
        }

        var result = await _signInManager.UserManager.ConfirmEmailAsync(user, token);
        
        if (!result.Succeeded)
        {
            _logger.LogError("Confirming email failed for user {Username} with ID: {Id}", user.UserName!, userId);
            throw new BadRequestException(
                _identityResult.ToValidationErrors(result),
                $"Failed to confirm email for { user.Email }"
            );
        }

        _logger.LogInformation("Confirming email successful for user {Username} with ID: {Id}", user.UserName!, userId);
    }

    public async Task ChangePassword(PasswordRequest request)
    {
        var user = _userService.User
            ?? throw new NotFoundException("No user found in current context");

        var applicationUser = await _signInManager.UserManager.GetUserAsync(user)
            ?? throw new NotFoundException("No user found");

        _logger.LogInformation("Changing password started for user {Username} with ID: {Id}", applicationUser.UserName!, applicationUser.Id);

        var result = await _signInManager.UserManager.ChangePasswordAsync(
            applicationUser,
            request.CurrentPassword,
            request.NewPassword
        );

        if (!result.Succeeded)
        {
            _logger.LogError("Changing password failed for user {Username} with ID: {Id}", applicationUser.UserName!, applicationUser.Id);
            throw new BadRequestException(
                _identityResult.ToValidationErrors(result),
                $"Failed to change password for user { applicationUser.Email }"
            );
        }

        _logger.LogInformation("Changing password successful for user {Username} with ID: {Id}", applicationUser.UserName!, applicationUser.Id);
    }
}
