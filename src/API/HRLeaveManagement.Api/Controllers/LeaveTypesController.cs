using HRLeaveManagement.Application.Features.LeaveType.Commands;
using HRLeaveManagement.Application.Features.LeaveType.Queries;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using HRLeaveManagement.Application.DTOs;

namespace HRLeaveManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class LeaveTypesController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeaveTypeDTO>>> GetAll()
    {
        var leaveTypeDtos = await _sender.Send(new GetAllLeaveTypesQuery());
        return Ok(leaveTypeDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LeaveTypeDetailsDTO>> GetById([FromRoute] int id)
    {
        var leaveTypeDetailsDto = await _sender.Send(new GetLeaveTypeDetailsQuery(id));
        return Ok(leaveTypeDetailsDto);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateLeaveTypeCommand command)
    {
        var resultId = await _sender.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = resultId }, command);
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
