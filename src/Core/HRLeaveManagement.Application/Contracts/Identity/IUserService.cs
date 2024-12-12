using HRLeaveManagement.Application.DTOs.Identity;
using System.Security.Claims;

namespace HRLeaveManagement.Application.Contracts.Identity;

public interface IUserService
{
    ClaimsPrincipal? User { get; }
    string? UserId { get; }
    bool IsUserLoggedIn { get; }
    Task<IEnumerable<UserDTO>> GetUsers();
    Task<UserDTO> GetUser(string userId);
}
