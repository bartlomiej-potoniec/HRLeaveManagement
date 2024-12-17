using Microsoft.AspNetCore.Authorization;

namespace HRLeaveManagement.Api.Authorization.Requirements;

public sealed class IsEmployeeRequirement : IAuthorizationRequirement
{
    public IsEmployeeRequirement() {}
}
