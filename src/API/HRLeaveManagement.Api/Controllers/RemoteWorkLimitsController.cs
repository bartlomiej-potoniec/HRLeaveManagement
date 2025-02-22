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
    public async Task<ActionResult<IEnumerable<RemoteWorkLimitDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var remoteWorkLimits = await _sender.Send(new GetAllRemoteWorkLimitsQuery(), cancellationToken);
        return Ok(remoteWorkLimits);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RemoteWorkLimitDetailsDTO>> GetWithDetails([FromRoute] int id,
                                                                              CancellationToken cancellationToken)
    {
        var remoteWorkLimit = await _sender.Send(new GetRemoteWorkLimitWithDetailsQuery(id), cancellationToken);

        return Ok(remoteWorkLimit);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateRemoteWorkLimitCommand command,
                                                CancellationToken cancellationToken)
    {
        var remoteWorkLimitId = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetWithDetails), new { remoteWorkLimitId }, command);
    }

    [HttpPut("{remoteWorkLimitId}")]
    public async Task<ActionResult> Update([FromRoute] int remoteWorkLimitId,
                                           [FromBody] UpdateRemoteWorkLimitCommand command,
                                           CancellationToken cancellationToken)
    {
        await _sender.Send(command with { Id = remoteWorkLimitId }, cancellationToken);
        return NoContent();
    }
}
