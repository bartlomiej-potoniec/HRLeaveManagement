using HRLeaveManagement.Application.DTOs.RemoteWorkLimits;
using HRLeaveManagement.Application.Features.RemoteWorkLimit.Queries;
using HRLeaveManagement.Application.Features.RemoteWorkLimit.Commands;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace HRLeaveManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class RemoteWorkLimitsController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RemoteWorkLimitDTO>>> GetAll()
    {
        var remoteWorkLimits = await _sender.Send(new GetAllRemoteWorkLimitsQuery());
        return Ok(remoteWorkLimits);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RemoteWorkLimitDetailsDTO>> GetWithDetails([FromRoute] int id)
    {
        var remoteWorkLimit = await _sender.Send(new GetRemoteWorkLimitWithDetailsQuery(id));
        return Ok(remoteWorkLimit);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateRemoteWorkLimitCommand command)
    {
        var remoteWorkLimitId = await _sender.Send(command);
        return CreatedAtAction(nameof(GetWithDetails), new { remoteWorkLimitId }, command);
    }

    [HttpPut("{remoteWorkLimitId}")]
    public async Task<ActionResult> Update([FromRoute] int remoteWorkLimitId,
                                           [FromBody] UpdateRemoteWorkLimitCommand command)
    {
        await _sender.Send(command with { Id = remoteWorkLimitId });
        return NoContent();
    }
}
