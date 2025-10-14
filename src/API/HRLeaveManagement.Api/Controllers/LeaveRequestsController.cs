using HRLeaveManagement.Application.Features.LeaveRequest.Commands;
using HRLeaveManagement.Application.Features.LeaveRequest.Queries;
using HRLeaveManagement.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace HRLeaveManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class LeaveRequestsController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<LeaveRequestDTO>>> GetAllWithDetails(CancellationToken cancellationToken)
    {
        var leaveRequests = await _sender.Send(
            new GetAllLeaveRequestsWithDetailsQuery(),
            cancellationToken
        );

        return Ok(leaveRequests);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<LeaveRequestDetailsDTO>> GetWithDetailsById([FromRoute] int id,
                                                                               CancellationToken cancellationToken)
    {
        var leaveRequest = await _sender.Send(new GetLeaveRequestWithDetailsByIdQuery(id), cancellationToken);
        return Ok(leaveRequest);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Create([FromBody] CreateLeaveRequestCommand command,
                                           CancellationToken cancellationToken)
    {
        var leaveRequestId = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetWithDetailsById), new { id = leaveRequestId }, command);
    }

    [HttpPatch("{id:int}/approve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Approve(int id,
                                            [FromBody] ApproveLeaveRequestCommand command,
                                            CancellationToken cancellationToken)
    {
        await _sender.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:int}/reject")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Reject([FromRoute] int id,
                                           [FromBody] RejectLeaveRequestCommand command,
                                           CancellationToken cancellationToken)
    {
        await _sender.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:int}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Cancel([FromRoute] int id,
                                       [FromBody] CancelLeaveRequestCommand command,
                                       CancellationToken cancellationToken)
    {
        await _sender.Send(command, cancellationToken);
        return NoContent();
    }
}
