using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.DTOs.Users;
using System.Security.Claims;

namespace HRLeaveManagement.Application.Contracts.Identity;

public interface IUserService
{
    ClaimsPrincipal? User { get; }
    string? UserId { get; }
    string? UserName { get; }
    bool IsUserLoggedIn { get; }
    
    bool IsUserInRole(string roleName);
    Task<bool> IsUserEmployeeByEmployeeId(Guid employeeId);
    Task<bool> IsUserInManagerRoleByEmployeeId(Guid employeeId);

    Task<IEnumerable<UserDTO>> GetAllUsers();
    Task<PagedResult<UserDetailsDTO>> GetAllPagedUsers(int? pageSize,
                                                               int? pageNumber,
                                                               string? sorts,
                                                               string? filters);
    Task<UserDetailsDTO> GetUserWithDetailsById(Guid id);
    Task<IEnumerable<UserDTO>> GetAllUsersInRole(string role);
    Task<UserDTO> GetUserById(Guid id);
    Task<UserDTO> GetUserByEmployeeId(Guid employeeId);

    Task Update(UpdateUserRequest request);
    Task UpdateUserEmployeeId(Guid userId, Guid employeeId);
    Task LockoutUserAccountById(LockoutUserAccountRequest request);
    Task UnlockUserAccountById(UnlockUserAccountRequest request);
}
