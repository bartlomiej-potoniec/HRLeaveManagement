using HRLeaveManagement.Application.Features.LeaveRequest.Commands;
using HRLeaveManagement.Application.Features.LeaveRequest.Queries;
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
    public async Task<IActionResult> GetAllWithDetails()
    {
        var leaveRequests = await _sender.Send(new GetAllLeaveRequestsWithDetailsQuery());
        return Ok(leaveRequests);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetWithDetailsById([FromRoute] int id)
    {
        var leaveRequest = await _sender.Send(new GetLeaveRequestWithDetailsByIdQuery(id));
        return Ok(leaveRequest);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateLeaveRequestCommand command)
    {
        var leaveRequestId = await _sender.Send(command);
        return CreatedAtAction(nameof(GetWithDetailsById), new { id = leaveRequestId }, command);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update([FromRoute] int id,
                                            [FromBody] UpdateLeaveRequestCommand command)
    {
        await _sender.Send(command with { Id = id });
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _sender.Send(new DeleteLeaveRequestCommand(id));
        return NoContent();
    }

    [HttpPatch("cancel")]
    public async Task<IActionResult> Cancel([FromBody] CancelLeaveRequestCommand command)
    {
        await _sender.Send(command);
        return NoContent();
    }

    [HttpPatch("approve")]
    public async Task<IActionResult> Approve([FromBody] ChangeLeaveRequestApprovalCommand command)
    {
        await _sender.Send(command);
        return NoContent();
    }
}
