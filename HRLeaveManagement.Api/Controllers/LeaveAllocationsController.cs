using HRLeaveManagement.Application.Features.LeaveAllocation.Commands;
using HRLeaveManagement.Application.Features.LeaveAllocation.Queries;
using HRLeaveManagement.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace HRLeaveManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveAllocationsController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<LeaveAllocationDTO>>> GetAll()
    {
        var leaveAllocations = await _sender.Send(new GetAllLeaveAllocationsQuery());
        return Ok(leaveAllocations);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<LeaveAllocationDetailsDTO>> GetById([FromRoute] int id)
    {
        var leaveAllocation = await _sender.Send(new GetLeaveAllocationDetailsQuery(id));
        return Ok(leaveAllocation);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Create([FromBody] CreateLeaveAllocationCommand command)
    {
        var resultId = await _sender.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = resultId }, command);
    }

    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Update([FromRoute] int id,
                                            [FromBody] UpdateLeaveAllocationCommand command)
    {
        await _sender.Send(command with { Id = id });
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        await _sender.Send(new DeleteLeaveAllocationCommand(id));
        return NoContent();
    }
}
