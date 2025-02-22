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
                                                                                   [FromQuery] string? filters,
                                                                                   CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllPagedUsersAsync(
            pageSize,
            pageNumber,
            sorts,
            filters,
            cancellationToken
        );

        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Administrator,HR")]
    public async Task<ActionResult<UserDetailsDTO>> GetWithDetails([FromRoute] Guid id,
                                                                   CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserWithDetailsByIdAsync(id, cancellationToken);
        return Ok(user);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update([FromRoute] Guid id,
                                           [FromBody] UpdateUserRequest request,
                                           CancellationToken cancellationToken)
    {
        await _userService.UpdateAsync(request with { Id = id }, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id}/lockout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> LockoutAccount([FromRoute] Guid id,
                                                   [FromBody] LockoutUserAccountRequest request,
                                                   CancellationToken cancellationToken)
    {
        await _userService.LockoutUserAccountByIdAsync(request with { UserId = id }, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id}/unlock")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UnlockAccount([FromRoute] Guid id,
                                                  [FromBody] UnlockUserAccountRequest request,
                                                  CancellationToken cancellationToken)
    {
        await _userService.UnlockUserAccountByIdAsync(request with { UserId = id }, cancellationToken);
        return NoContent();
    }
}
