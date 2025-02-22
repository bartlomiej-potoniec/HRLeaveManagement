using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Identity.DbContexts;
using HRLeaveManagement.Identity.Models;
using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.DTOs.Users;
using HRLeaveManagement.Infrastructure.Sieve.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Data;
using Sieve.Services;

namespace HRLeaveManagement.Identity.Services;

public sealed class UserService(UserManager<ApplicationUser> userManager,
                                IServiceProvider serviceProvider,
                                IHttpContextAccessor httpContextAccessor,
                                IServiceScopeFactory serviceScopeFactory,
                                ISieveProcessor sieveProcessor,
                                IIdentityResult identityResult,
                                ILogger<UserService> logger)
    : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly ISieveProcessor _sieveProcessor = sieveProcessor;
    private readonly IIdentityResult _identityResult = identityResult;
    private readonly ILogger<UserService> _logger = logger;

    private readonly ApplicationIdentityDbContext _dbContext
        = serviceProvider.GetRequiredService<ApplicationIdentityDbContext>();

    public ClaimsPrincipal? User => _httpContextAccessor?.HttpContext?.User;

    public string? UserId 
        => User?.FindFirst(claim => claim.Type is "uid")?.Value;

    public string? UserName
        => User?.FindFirst(claim => claim.Type is JwtRegisteredClaimNames.UniqueName)?.Value;

    public bool IsUserLoggedIn 
        => User?.Identity is not null && User.Identity.IsAuthenticated;

    public bool IsUserInRole(string roleName) => User?.IsInRole(roleName) is not null;

    public async Task<bool> IsUserEmployeeByEmployeeIdAsync(Guid employeeId,
                                                            CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId, cancellationToken);

        return user is not null;
    }

    public async Task<bool> IsUserInManagerRoleByEmployeeIdAsync(Guid employeeId,
                                                                 CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching user with employee ID: {Id} started", employeeId);

        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId, cancellationToken)
            ?? throw new NotFoundException($"No user with employee ID: { employeeId } found");

        _logger.LogInformation("Fetching user with employee ID: {Id} successful", employeeId);
        _logger.LogInformation("Checking user with ID: {Id} to be in role Manager started", employeeId);

        var result = await _userManager.IsInRoleAsync(user, "Manager");

        _logger.LogInformation("Checking user with ID: {Id} to be in role Manager successful", employeeId);

        return result;
    }

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
            ?? throw new NotFoundException($"No user with ID: { id } found");

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
            ?? throw new NotFoundException($"No user with employee ID: { employeeId } found");

        var userDto = ApplicationUser.CreateUserDTO(user);

        _logger.LogInformation("Fetching user with employee ID: {Id} successful", employeeId);

        return userDto;
    }

    public async Task UpdateUserEmployeeIdAsync(Guid userId,
                                                Guid employeeId,
                                                CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating user with ID: {UserId} with employee ID: {EmployeeId} started", userId, employeeId);

        var user = await _userManager
            .FindByIdAsync(userId.ToString())
            ?? throw new NotFoundException($"No user with ID: {userId} found");

        ApplicationUser.UpdateUserEmployeeId(user, employeeId);
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            _logger.LogError("Updating user with ID: {UserId} with employee ID: {EmployeeId} failed", user.Id, employeeId);
            throw new BadRequestException(
                _identityResult.ToValidationErrors(result),
                $"Failed to update user with ID {user.Id}"
            );
        }

        _logger.LogInformation("Updating user with ID: {UserId} with employee ID: {EmployeeId} successful", userId, employeeId);
    }

    public async Task UpdateAsync(UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager
            .FindByIdAsync(request.Id.ToString())
            ?? throw new NotFoundException($"No user with ID: { request.Id } found");

        ApplicationUser.Update(user, request);
        
        _logger.LogInformation("Starting transaction for updating user with ID {UserId}", user.Id);
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            _logger.LogInformation("Updating info started for user with ID {UserId}", user.Id);
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogError("Updating info for user with ID: {UserId} failed", user.Id);
                throw new BadRequestException(
                    _identityResult.ToValidationErrors(result),
                    $"Failed to update user with ID {user.Id}"
                );
            }

            _logger.LogInformation("Updating info successful for user with ID {UserId}", user.Id);

            var currentRoles = await _userManager.GetRolesAsync(user);

            _logger.LogInformation("Removing current roles {Roles} started for user with ID {UserId}", currentRoles, user.Id);
            result = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!result.Succeeded)
            {
                _logger.LogError("Removing current roles {Roles} for user with ID: {UserId} failed", currentRoles, user.Id);
                throw new BadRequestException(
                    _identityResult.ToValidationErrors(result),
                    $"Failed to remove current roles for user with ID {user.Id}"
                );
            }

            _logger.LogInformation("Removing current roles {Roles} successful for user with ID {UserId}", currentRoles, user.Id);
            _logger.LogInformation("Adding new roles {Roles} started for user with ID {UserId}", request.Roles, user.Id);

            result = await _userManager.AddToRolesAsync(user, request.Roles);

            if (!result.Succeeded)
            {
                _logger.LogError("Adding new roles {Roles} for user with ID: {UserId} failed", request.Roles, user.Id);
                throw new BadRequestException(
                    _identityResult.ToValidationErrors(result),
                    $"Failed to add new roles for user with ID {user.Id}"
                );
            }

            _logger.LogInformation("Adding new roles {Roles} successfull for user with ID {UserId}", request.Roles, user.Id);

            await transaction.CommitAsync(cancellationToken);
            _logger.LogInformation("Transaction successful for updating user with ID {UserId}", user.Id);
        }

        catch (Exception ex)
        {
            _logger.LogError("Transaction failed for updating user with ID {UserId}", user.Id);
            await transaction.RollbackAsync(cancellationToken);

            throw;
        }
    }

    public async Task LockoutUserAccountByIdAsync(LockoutUserAccountRequest request,
                                                  CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Locking user account with ID: {UserId} started", request.UserId);

        var user = await _userManager
            .FindByIdAsync(request.UserId.ToString())
            ?? throw new NotFoundException($"No user with ID: { request.UserId } found");

        ApplicationUser.LockoutUserUntilDateTime(user, request.LockoutEnd);
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            _logger.LogError("Locking user account with ID: {UserId} failed", request.UserId);
            throw new BadRequestException(
                _identityResult.ToValidationErrors(result),
                $"Failed to lock out the user with ID {user.Id}"
            );
        }

        _logger.LogInformation("Locking user account with ID: {UserId} successful", request.UserId);
    }

    public async Task UnlockUserAccountByIdAsync(UnlockUserAccountRequest request,
                                                 CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Unlocking user account with ID: {UserId} started", request.UserId);

        var user = await _userManager
            .FindByIdAsync(request.UserId.ToString())
            ?? throw new NotFoundException($"No user with ID: {request.UserId} found");

        ApplicationUser.UnlockUser(user);
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            _logger.LogError("Unlocking user account with ID: {UserId} failed", request.UserId);
            throw new BadRequestException(
                _identityResult.ToValidationErrors(result),
                $"Failed to unlock the user with ID {user.Id}"
            );
        }

        _logger.LogInformation("Unlocking user account with ID: {UserId} successful", request.UserId);
    }
}
