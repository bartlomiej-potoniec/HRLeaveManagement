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
    public async Task<ActionResult<IEnumerable<LeaveTypeDTO>>> GetAll()
    {
        var leaveTypes = await _sender.Send(new GetAllLeaveTypesQuery());
        return Ok(leaveTypes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LeaveTypeDetailsDTO>> GetWithDetails([FromRoute] int id)
    {
        var leaveTypeDetails = await _sender.Send(new GetLeaveTypeWithDetailsQuery(id));
        return Ok(leaveTypeDetails);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateLeaveTypeCommand command)
    {
        var leaveTypeId = await _sender.Send(command);
        return CreatedAtAction(nameof(GetWithDetails), new { id = leaveTypeId }, command);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Update([FromRoute] int id,
                                           [FromBody] UpdateLeaveTypeCommand command)
    {
        await _sender.Send(command with { Id = id });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        await _sender.Send(new DeleteLeaveTypeCommand(id));
        return NoContent();
    }
}
