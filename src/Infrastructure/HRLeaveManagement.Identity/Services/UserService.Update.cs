using HRLeaveManagement.Application.DTOs.Users;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HRLeaveManagement.Identity.Services;

public sealed partial class UserService
{
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
            ?? throw new NotFoundException($"No user with ID: {request.Id} found");

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
            ?? throw new NotFoundException($"No user with ID: {request.UserId} found");

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

    public async Task LockoutUserAccountsAsync(LockoutManyUserAccountsRequest request,
                                               CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Locking user accounts with ID: {UserId} started", request.UserIds);
        _logger.LogInformation("Starting transaction for locking users with ID {UserId}", request.UserIds);

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await Parallel.ForEachAsync(request.UserIds, cancellationToken, async (id, token) =>
            {
                if (token.IsCancellationRequested) return;

                using var scope = _serviceScopeFactory.CreateScope();
                var scopedUserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                _logger.LogInformation("Locking user account with ID: {UserId} started", id);

                var user = await scopedUserManager
                    .FindByIdAsync(id.ToString())
                    ?? throw new NotFoundException($"No user with ID: {id} found");

                ApplicationUser.LockoutUserUntilDateTime(user, request.LockoutEnd);
                var result = await scopedUserManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    _logger.LogError("Locking user account with ID: {UserId} failed", id);
                    throw new BadRequestException(
                        _identityResult.ToValidationErrors(result),
                        $"Failed to lock out the user with ID {id}"
                    );
                }

                _logger.LogInformation("Locking user account with ID: {UserId} successful", id);
            });

            await transaction.CommitAsync(cancellationToken);
            _logger.LogInformation("Transaction successful for locking users with ID {UserId}", request.UserIds);
        }

        catch (Exception ex)
        {
            _logger.LogError("Transaction failed for unlocking users with ID {UserId}", request.UserIds);
            await transaction.RollbackAsync(cancellationToken);

            throw;
        }

        _logger.LogInformation("Unlocking user accounts with ID: {UserId} successful", request.UserIds);
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

    public async Task UnlockUserAccountsAsync(UnlockManyUserAccountsRequest request,
                                              CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Unlocking user accounts with ID: {UserId} started", request.UserIds);
        _logger.LogInformation("Starting transaction for unlocking users with ID {UserId}", request.UserIds);

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await Parallel.ForEachAsync(request.UserIds, cancellationToken, async (id, token) =>
            {
                if (token.IsCancellationRequested) return;

                using var scope = _serviceScopeFactory.CreateScope();
                var scopedUserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                _logger.LogInformation("Unlocking user account with ID: {UserId} started", id);

                var user = await scopedUserManager
                    .FindByIdAsync(id.ToString())
                    ?? throw new NotFoundException($"No user with ID: {id} found");

                ApplicationUser.UnlockUser(user);
                var result = await scopedUserManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    _logger.LogError("Unlocking user account with ID: {UserId} failed", id);
                    throw new BadRequestException(
                        _identityResult.ToValidationErrors(result),
                        $"Failed to unlock the user with ID {id}"
                    );
                }

                _logger.LogInformation("Unlocking user account with ID: {UserId} successful", id);
            });

            await transaction.CommitAsync(cancellationToken);
            _logger.LogInformation("Transaction successful for unlocking users with ID {UserId}", request.UserIds);
        }

        catch (Exception ex)
        {
            _logger.LogError("Transaction failed for unlocking users with ID {UserId}", request.UserIds);
            await transaction.RollbackAsync(cancellationToken);

            throw;
        }

        _logger.LogInformation("Unlocking user accounts with ID: {UserId} successful", request.UserIds);
    }
}
