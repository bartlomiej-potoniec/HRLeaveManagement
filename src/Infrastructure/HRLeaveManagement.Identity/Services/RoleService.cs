using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.DTOs.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Identity.Services;

public sealed class RoleService(RoleManager<IdentityRole> roleManager, 
                                IAppLogger<RoleService> logger) 
    : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IAppLogger<RoleService> _logger = logger;

    public async Task<IEnumerable<RoleDTO>> GetAllRoles()
    {
        _logger.LogInformation("Fetching Identity roles started");

        var identityRoles = await _roleManager.Roles.ToListAsync();
        
        var roles = identityRoles
            .Select(ConvertIdentityRoleToRoleDTO)
            .ToList();

        _logger.LogInformation("Fetching Identity roles started");

        return roles;
    }

    private RoleDTO ConvertIdentityRoleToRoleDTO(IdentityRole role)
        => new()
        {
            Id = Guid.Parse(role.Id),
            Name = role.Name!
        };
}
