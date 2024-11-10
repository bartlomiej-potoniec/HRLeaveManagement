using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.DTOs.Identity;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HRLeaveManagement.Identity.Services;

public sealed class UserService(UserManager<ApplicationUser> userManager,
                                IHttpContextAccessor httpContextAccessor)
    : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public ClaimsPrincipal? User => _httpContextAccessor?.HttpContext?.User;

    public string? UserId 
        => User?.FindFirst(claim => claim.Type is "uid")?.Value;

    public bool IsUserLoggedIn 
        => User?.Identity is not null && User.Identity.IsAuthenticated;

    public bool IsUserInRole(string roleName) => User?.IsInRole(roleName) is not null;
    
    public async Task<UserDTO> GetUser(string userId)
    {
        var employee = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException($"user with id { userId } not found");

        return new(employee.Id, employee.Email!, employee.FirstName, employee.LastName);
    }

    public async Task<IEnumerable<UserDTO>> GetUsers()
    {
        var employees = await _userManager.GetUsersInRoleAsync("Employee");

        var employeesList = employees
            .Select(e => new UserDTO(e.Id, e.Email!, e.FirstName, e.LastName))
            .ToList();

        return employeesList;
    }
}
