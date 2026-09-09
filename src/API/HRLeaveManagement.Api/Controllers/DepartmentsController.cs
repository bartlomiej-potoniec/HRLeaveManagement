using HRLeaveManagement.Application.Features.Department.Queries;
using HRLeaveManagement.Application.DTOs.Departments;
using HRLeaveManagement.Application.DTOs.Sections;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using HRLeaveManagement.Application.Features.Department.CreateDepartment.Commands;
using HRLeaveManagement.Application.Features.Department.UpdateDepartment.Commands;

namespace HRLeaveManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class DepartmentsController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var departments = await _sender.Send(new GetAllDepartmentsQuery(), cancellationToken);
        return Ok(departments);
    }

    [HttpGet("{departmentId}")]
    public async Task<ActionResult<DepartmentDetailsDTO>> GetWithDetails([FromRoute] int departmentId,
                                                                         CancellationToken cancellationToken)
    {
        var department = await _sender.Send(new GetDepartmentWithDetailsQuery(departmentId), cancellationToken);
        return Ok(department);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateDepartmentCommand command,
                                                CancellationToken cancellationToken)
    {
        var departmentId = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetWithDetails), new { departmentId }, command);
    }

    [HttpPut("{departmentId}")]
    public async Task<ActionResult> Update([FromRoute] int departmentId,
                                           [FromBody] UpdateDepartmentCommand command,
                                           CancellationToken cancellationToken)
    {
        await _sender.Send(command with { Id = departmentId }, cancellationToken);
        return NoContent();
    }

    [HttpGet("{departmentId}/sections")]
    public async Task<ActionResult<IEnumerable<SectionDTO>>> GetAllSectionsByDepartmentId([FromRoute] int departmentId,
                                                                                          CancellationToken cancellationToken)
    {
        var sections = await _sender.Send(new GetAllSectionsByDepartmentIdQuery(departmentId), cancellationToken);
        return Ok(sections);
    }
}
