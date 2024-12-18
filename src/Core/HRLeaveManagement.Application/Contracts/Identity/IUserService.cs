using HRLeaveManagement.Application.DTOs.User;
using System.Security.Claims;

namespace HRLeaveManagement.Application.Contracts.Identity;

public interface IUserService
{
    ClaimsPrincipal? User { get; }
    string? UserId { get; }
    string? UserName { get; }
    bool IsUserLoggedIn { get; }
    
    bool IsUserInRole(string roleName);
    Task<bool> IsUserInRole(Guid userId, string roleName);
    Task<bool> IsUserInManagerRoleByEmployeeId(Guid employeeId);

    Task<IEnumerable<UserDTO>> GetAllUsersInRole(string role);
    Task<UserDTO> GetUserById(Guid id);
    Task<UserDTO> GetUserByEmployeeId(Guid employeeId);
    Task UpdateUserEmployeeId(Guid userId, Guid employeeId);
}
