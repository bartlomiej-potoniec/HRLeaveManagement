using HRLeaveManagement.Application.Models.Identity;

namespace HRLeaveManagement.Application.Contracts.Identity;

public interface IUserService
{
    Task<IEnumerable<Employee>> GetEmployees();
    Task<Employee> GetEmployee(string userId);
}
