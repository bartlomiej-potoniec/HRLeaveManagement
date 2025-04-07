using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.DTOs.Users;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Identity.Models;
using HRLeaveManagement.Infrastructure.Sieve.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace HRLeaveManagement.Identity.Services;

public sealed partial class UserService
{

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all users started");

        var users = await _userManager.Users.ToListAsync(cancellationToken);

        var userDtos = users
            .Select(ApplicationUser.CreateUserDTO)
            .ToList();

        _logger.LogInformation("Fetching all users successful");

        return userDtos;
    }

    public async Task<PagedResult<UserDetailsDTO>> GetAllPagedUsersAsync(int? pageSize = null,
                                                                         int? pageNumber = null,
                                                                         string? sorts = null,
                                                                         string? filters = null,
                                                                         CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all users with details started");

        var users = await _userManager.Users.ToListAsync(cancellationToken);
        var userDtos = new ConcurrentBag<UserDetailsDTO>();

        await Parallel.ForEachAsync(users, cancellationToken, async (user, token) =>
        {
            if (token.IsCancellationRequested) return;

            using var scope = _serviceScopeFactory.CreateScope();
            var scopedUserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var userRoles = await scopedUserManager.GetRolesAsync(user);
            var userDto = ApplicationUser.CreateUserDetailsDTO(user, userRoles);

            userDtos.Add(userDto);
        });

        _logger.LogInformation("Fetching all users with details successful");
        _logger.LogInformation("Paginating result started");

        var queryableUserDtos = userDtos.AsQueryable();
        var model = SieveModelMapper.Map(pageSize, pageNumber, sorts, filters);

        var result = _sieveProcessor
            .Apply(model, queryableUserDtos)
            .ToList();

        var totalCount = queryableUserDtos.Count();

        var paginatedResult = new PagedResult<UserDetailsDTO>(result, totalCount, pageSize, pageNumber);

        _logger.LogInformation("Paginating result successfull");

        return paginatedResult;
    }

    public async Task<UserDetailsDTO> GetUserWithDetailsByIdAsync(Guid id,
                                                                  CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching user with ID: {Id} with details started", id);

        var user = await _userManager
            .FindByIdAsync(id.ToString())
            ?? throw new NotFoundException($"No user with ID: {id} found");

        var userRoles = await _userManager.GetRolesAsync(user);

        var userDetailsDto = ApplicationUser.CreateUserDetailsDTO(user, userRoles);

        _logger.LogInformation("Fetching user with ID: {Id} with details successful", id);

        return userDetailsDto;
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsersInRoleAsync(string role,
                                                                   CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all users in role {Role} started", role);

        var users = await _userManager.GetUsersInRoleAsync(role);

        var userDtos = users
            .Select(ApplicationUser.CreateUserDTO)
            .ToList();

        _logger.LogInformation("Fetching all users in role {Role} successful", role);

        return userDtos;
    }

    public async Task<UserDTO> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching user with ID: {Id} started", id);

        var user = await _userManager
            .FindByIdAsync(id.ToString())
            ?? throw new NotFoundException($"No user with ID: {id} found");

        var userDto = ApplicationUser.CreateUserDTO(user);

        _logger.LogInformation("Fetching user with ID: {Id} successful", id);

        return userDto;
    }

    public async Task<UserDTO> GetUserByEmployeeIdAsync(Guid employeeId,
                                                        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching user with employee ID: {Id} started", employeeId);

        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId, cancellationToken)
            ?? throw new NotFoundException($"No user with employee ID: {employeeId} found");

        var userDto = ApplicationUser.CreateUserDTO(user);

        _logger.LogInformation("Fetching user with employee ID: {Id} successful", employeeId);

        return userDto;
    }
}
