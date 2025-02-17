using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRLeaveManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrator,HR")]
public sealed class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet]
    [Authorize(Roles = "Administrator,HR")]
    public async Task<ActionResult<PagedResult<UserDetailsDTO>>> GetAllWithDetails([FromQuery] int? pageNumber,
                                                                                   [FromQuery] int? pageSize,
                                                                                   [FromQuery] string? sorts,
                                                                                   [FromQuery] string? filters)
    {
        var users = await _userService.GetAllPagedUsers(pageSize, pageNumber, sorts, filters);
        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Administrator,HR")]
    public async Task<ActionResult<UserDetailsDTO>> GetWithDetails([FromRoute] Guid id)
    {
        var user = await _userService.GetUserWithDetailsById(id);
        return Ok(user);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update([FromRoute] Guid id,
                                           [FromBody] UpdateUserRequest request)
    {
        await _userService.Update(request with { Id = id });
        return NoContent();
    }

    [HttpPatch("{id}/lockout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> LockoutAccount([FromRoute] Guid id,
                                                   [FromBody] LockoutUserAccountRequest request)
    {
        await _userService.LockoutUserAccountById(request with { UserId = id });
        return NoContent();
    }

    [HttpPatch("{id}/unlock")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UnlockAccount([FromRoute] Guid id,
                                                   [FromBody] UnlockUserAccountRequest request)
    {
        await _userService.UnlockUserAccountById(request with { UserId = id });
        return NoContent();
    }
}
