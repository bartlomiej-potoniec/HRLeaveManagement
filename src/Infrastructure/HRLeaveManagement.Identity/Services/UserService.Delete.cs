using HRLeaveManagement.Identity.Models;
using HRLeaveManagement.Application.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HRLeaveManagement.Identity.Services;

public sealed partial class UserService
{
    public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting user account with ID: {UserId} started", id);

        var user = await _userManager
            .FindByIdAsync(id.ToString())
            ?? throw new NotFoundException($"No user with ID: {id} found");

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            _logger.LogError("Deleting user account with ID: {UserId} failed", id);
            throw new BadRequestException(
                _identityResult.ToValidationErrors(result),
                $"Failed to detele the user with ID {id}"
            );
        }

        _logger.LogInformation("Deleting user account with ID: {UserId} successful", id);
    }

    public async Task DeleteUsersAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting user accounts with ID: {UserId} started", ids);
        _logger.LogInformation("Starting transaction for deleting users with ID {UserId}", ids);

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await Parallel.ForEachAsync(ids, cancellationToken, async (id, token) =>
            {
                if (token.IsCancellationRequested) return;

                using var scope = _serviceScopeFactory.CreateScope();
                var scopedUserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                _logger.LogInformation("Deleting user account with ID: {UserId} started", id);

                var user = await scopedUserManager
                    .FindByIdAsync(id.ToString())
                    ?? throw new NotFoundException($"No user with ID: {id} found");

                var result = await scopedUserManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    _logger.LogError("Deleting user account with ID: {UserId} failed", id);
                    throw new BadRequestException(
                        _identityResult.ToValidationErrors(result),
                        $"Failed to detele the user with ID {id}"
                    );
                }

                _logger.LogInformation("Deleting user account with ID: {UserId} successful", id);
            });

            await transaction.CommitAsync(cancellationToken);
            _logger.LogInformation("Transaction successful for updating user with ID {UserId}", ids);
        }

        catch (Exception ex)
        {
            _logger.LogError("Transaction failed for deleting users with ID {UserId}", ids);
            await transaction.RollbackAsync(cancellationToken);

            throw;
        }

        _logger.LogInformation("Deleting user accounts with ID: {UserId} successful", ids);
    }
}
