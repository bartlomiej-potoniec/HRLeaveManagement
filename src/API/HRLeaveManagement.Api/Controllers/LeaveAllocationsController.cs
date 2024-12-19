using HRLeaveManagement.Application.Features.LeaveAllocation.Commands;
using HRLeaveManagement.Application.Features.LeaveAllocation.Queries;
using HRLeaveManagement.Application.DTOs.LeaveAllocations;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace HRLeaveManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class LeaveAllocationsController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeaveAllocationDTO>>> GetAll()
    {
        var leaveAllocations = await _sender.Send(new GetAllLeaveAllocationsQuery());
        return Ok(leaveAllocations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LeaveAllocationDetailsDTO>> GetWithDetails([FromRoute] int id)
    {
        var leaveAllocation = await _sender.Send(new GetLeaveAllocationWithDetailsQuery(id));
        return Ok(leaveAllocation);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateLeaveAllocationsCommand command)
    {
        await _sender.Send(command);
        return CreatedAtAction(nameof(GetAll), null, command);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Update([FromRoute] int id,
                                           [FromBody] UpdateLeaveAllocationCommand command)
    {
        await _sender.Send(command with { Id = id });
        return NoContent();
    }
}
