using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Identity.Models;
using HRLeaveManagement.Application.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace HRLeaveManagement.Identity.Services;

public sealed class UserService(UserManager<ApplicationUser> userManager,
                                IHttpContextAccessor httpContextAccessor,
                                ILogger<UserService> logger)
    : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ILogger<UserService> _logger = logger;

    public ClaimsPrincipal? User => _httpContextAccessor?.HttpContext?.User;

    public string? UserId 
        => User?.FindFirst(claim => claim.Type is "uid")?.Value;

    public string? UserName
        => User?.FindFirst(claim => claim.Type is JwtRegisteredClaimNames.UniqueName)?.Value;

    public bool IsUserLoggedIn 
        => User?.Identity is not null && User.Identity.IsAuthenticated;

    public bool IsUserInRole(string roleName) => User?.IsInRole(roleName) is not null;

    public async Task<bool> IsUserInRole(Guid userId, string roleName)
    {
        _logger.LogInformation("Fetching user with ID: {Id} started", userId);

        var user = await _userManager
            .FindByIdAsync(userId.ToString())
            ?? throw new NotFoundException($"No user with ID: { userId } found");

        _logger.LogInformation("Fetching user with ID: {Id} successful", userId);
        _logger.LogInformation("Checking user with ID: {Id} to be in role {Role} started", userId, roleName);

        var result = await _userManager.IsInRoleAsync(user, roleName);

        _logger.LogInformation("Checking user with ID: {Id} to be in role {Role} successful", userId, roleName);

        return result;
    }

    public async Task<bool> IsUserInManagerRole(Guid employeeId)
    {
        _logger.LogInformation("Fetching user with employee ID: {Id} started", employeeId);

        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId)
            ?? throw new NotFoundException($"No user with employee ID: { employeeId } found");

        _logger.LogInformation("Fetching user with employee ID: {Id} successful", employeeId);
        _logger.LogInformation("Checking user with ID: {Id} to be in role Manager started", employeeId);

        var result = await _userManager.IsInRoleAsync(user, "Manager");

        _logger.LogInformation("Checking user with ID: {Id} to be in role Manager successful", employeeId);

        return result;
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsersInRole(string role)
    {
        _logger.LogInformation("Fetching all users started");

        var users = await _userManager.GetUsersInRoleAsync(role);

        var userList = users
            .Select(ConvertUserToUserDTO)
            .ToList();

        _logger.LogInformation("Fetching all users successful");
        
        return userList;
    }

    public async Task<UserDTO> GetUserById(Guid id)
    {
        _logger.LogInformation("Fetching user with ID: {Id} started", id);

        var user = await _userManager
            .FindByIdAsync(id.ToString())
            ?? throw new NotFoundException($"No user with ID: { id } found");

        var userDto = ConvertUserToUserDTO(user);

        _logger.LogInformation("Fetching user with ID: {Id} successful", id);

        return userDto;
    }

    public async Task<UserDTO> GetUserByEmployeeId(Guid employeeId)
    {
        _logger.LogInformation("Fetching user with employee ID: {Id} started", employeeId);

        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId)
            ?? throw new NotFoundException($"No user with employee ID: { employeeId } found");

        var userDto = ConvertUserToUserDTO(user);

        _logger.LogInformation("Fetching user with employee ID: {Id} successful", employeeId);

        return userDto;
    }

    public async Task UpdateUserEmployeeId(Guid userId, Guid employeeId)
    {
        _logger.LogInformation("Updating user with ID: {UserId} with employee ID: {EmployeeId} started", userId, employeeId);

        var user = await _userManager
            .FindByIdAsync(userId.ToString())
            ?? throw new NotFoundException($"No user with ID: { userId } found");

        user.EmployeeId = employeeId;
        await _userManager.UpdateAsync(user);

        _logger.LogInformation("Updating user with ID: {UserId} with employee ID: {EmployeeId} successful", userId, employeeId);
    }

    private UserDTO ConvertUserToUserDTO(ApplicationUser user)
        => new()
        {
            Id = Guid.Parse(user.Id),
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PeselNumber = user.PeselNumber,
            PhoneNumber = user.PhoneNumber!,
            DateOfBirth = user.DateOfBirth,
            EmployeeId = user.EmployeeId
        };
}
