using HRLeaveManagement.Application.Features.LeaveType.Commands;
using HRLeaveManagement.Application.Features.LeaveType.Queries;
using HRLeaveManagement.Application.DTOs.LeaveTypes;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace HRLeaveManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class LeaveTypesController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeaveTypeDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var leaveTypes = await _sender.Send(new GetAllLeaveTypesQuery(), cancellationToken);
        return Ok(leaveTypes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LeaveTypeDetailsDTO>> GetWithDetails([FromRoute] int id,
                                                                        CancellationToken cancellationToken)
    {
        var leaveTypeDetails = await _sender.Send(new GetLeaveTypeWithDetailsQuery(id), cancellationToken);
        return Ok(leaveTypeDetails);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateLeaveTypeCommand command,
                                           CancellationToken cancellationToken)
    {
        var leaveTypeId = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetWithDetails), new { id = leaveTypeId }, command);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Update([FromRoute] int id,
                                           [FromBody] UpdateLeaveTypeCommand command,
                                           CancellationToken cancellationToken)
    {
        await _sender.Send(command with { Id = id }, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteLeaveTypeCommand(id), cancellationToken);
        return NoContent();
    }
}
