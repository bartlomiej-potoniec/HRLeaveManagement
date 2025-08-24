using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Identity.DbContexts;
using HRLeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Sieve.Services;

namespace HRLeaveManagement.Identity.Services;

public sealed partial class UserService(UserManager<ApplicationUser> userManager,
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

    public string UserId => User?.FindFirst(claim => claim.Type is "uid")?.Value
        ?? throw new UnauthorizedAccessException("User is unathorized");

    public string UserName => User?.FindFirst(claim => claim.Type is JwtRegisteredClaimNames.UniqueName)?.Value
        ?? throw new UnauthorizedAccessException("User is unathorized");

    public Guid EmployeeId => Guid.Parse(
        User?.FindFirst(claim => claim.Type is "employeeId")?.Value
        ?? throw new UnauthorizedAccessException("Employee is unathorized")
    );

    public bool IsUserLoggedIn => User?.Identity is not null && User.Identity.IsAuthenticated;

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
}
