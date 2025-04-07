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
    Task<bool> IsUserEmployeeByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken);
    Task<bool> IsUserInManagerRoleByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken);

    Task<IEnumerable<UserDTO>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<PagedResult<UserDetailsDTO>> GetAllPagedUsersAsync(int? pageSize,
                                                       int? pageNumber,
                                                       string? sorts,
                                                       string? filters,
                                                       CancellationToken cancellationToken);
    Task<UserDetailsDTO> GetUserWithDetailsByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<UserDTO>> GetAllUsersInRoleAsync(string role, CancellationToken cancellationToken);
    Task<UserDTO> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<UserDTO> GetUserByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken);

    Task UpdateAsync(UpdateUserRequest request, CancellationToken cancellationToken);
    Task UpdateUserEmployeeIdAsync(Guid userId, Guid employeeId, CancellationToken cancellationToken);
    Task LockoutUserAccountByIdAsync(LockoutUserAccountRequest request, CancellationToken cancellationToken);
    Task LockoutUserAccountsAsync(LockoutManyUserAccountsRequest request, CancellationToken cancellationToken);
    Task UnlockUserAccountByIdAsync(UnlockUserAccountRequest request, CancellationToken cancellationToken);
    Task UnlockUserAccountsAsync(UnlockManyUserAccountsRequest request, CancellationToken cancellationToken);

    Task DeleteUserAsync(Guid id, CancellationToken cancellationToken);
    Task DeleteUsersAsync(List<Guid> ids, CancellationToken cancellationToken);
}
