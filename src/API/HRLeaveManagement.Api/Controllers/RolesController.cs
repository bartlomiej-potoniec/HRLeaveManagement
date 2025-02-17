using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.DTOs.Roles;
using Microsoft.AspNetCore.Mvc;

namespace HRLeaveManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class RolesController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDTO>>> GetAll()
    {
        var roles = await _roleService.GetAllRoles();
        return Ok(roles);
    }
}
