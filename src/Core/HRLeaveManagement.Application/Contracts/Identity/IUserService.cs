using HRLeaveManagement.Application.Models.Identity;
using System.Security.Claims;

namespace HRLeaveManagement.Application.Contracts.Identity;

public interface IUserService
{
    ClaimsPrincipal? User { get; }
    string? UserId { get; }
    bool IsUserLoggedIn { get; }
    Task<IEnumerable<Employee>> GetEmployees();
    Task<Employee> GetEmployee(string userId);
}
