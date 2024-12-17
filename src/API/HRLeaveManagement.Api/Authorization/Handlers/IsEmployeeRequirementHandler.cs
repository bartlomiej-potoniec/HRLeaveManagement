using HRLeaveManagement.Api.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HRLeaveManagement.Api.Authorization.Handlers;

public sealed class IsEmployeeRequirementHandler(IHttpContextAccessor httpContextAccessor)
    : AuthorizationHandler<IsEmployeeRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   IsEmployeeRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var routeId = httpContext?.Request.RouteValues["employeeId"]?.ToString();

        if (routeId is null)
            return Task.CompletedTask;

        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is not null && userId == routeId)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
