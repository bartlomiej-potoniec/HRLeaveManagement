using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Models.Identity;
using HRLeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace HRLeaveManagement.Identity.Services;

public sealed class UserService(UserManager<ApplicationUser> userManager) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Employee> GetEmployee(string userId)
    {
        var employee = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException($"user with id { userId } not found");

        return new(employee.Id, employee.Email!, employee.FirstName, employee.LastName);
    }

    public async Task<IEnumerable<Employee>> GetEmployees()
    {
        var employees = await _userManager.GetUsersInRoleAsync("Employee");

        var employeesList = employees
            .Select(e => new Employee(e.Id, e.Email!, e.FirstName, e.LastName))
            .ToList();

        return employeesList;
    }
}
