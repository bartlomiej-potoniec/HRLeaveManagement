using HRLeaveManagement.Application.Features.LeaveAllocation.Queries;
using HRLeaveManagement.Application.DTOs.LeaveAllocations;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using HRLeaveManagement.Application.Features.LeaveAllocation.CreateLeaveAllocation.Commands;
using HRLeaveManagement.Application.Features.LeaveAllocation.UpdateLeaveAllocation.Commands;

namespace HRLeaveManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class LeaveAllocationsController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeaveAllocationDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var leaveAllocations = await _sender.Send(new GetAllLeaveAllocationsQuery(), cancellationToken);
        return Ok(leaveAllocations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LeaveAllocationDetailsDTO>> GetWithDetails([FromRoute] int id,
                                                                              CancellationToken cancellationToken)
    {
        var leaveAllocation = await _sender.Send(new GetLeaveAllocationWithDetailsQuery(id), cancellationToken);
        return Ok(leaveAllocation);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateLeaveAllocationsCommand command,
                                           CancellationToken cancellationToken)
    {
        await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAll), null, command);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Update([FromRoute] int id,
                                           [FromBody] UpdateLeaveAllocationDaysCommand command,
                                           CancellationToken cancellationToken)
    {
        await _sender.Send(command with { Id = id }, cancellationToken);
        return NoContent();
    }
}
