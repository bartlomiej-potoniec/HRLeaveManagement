using HRLeaveManagement.Application.Features.Departments.Queries;
using HRLeaveManagement.Application.Features.Departments.Commands;
using HRLeaveManagement.Application.DTOs.Departments;
using HRLeaveManagement.Application.DTOs.Sections;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace HRLeaveManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetAll()
    {
        var departments = await _sender.Send(new GetAllDepartmentsQuery());
        return Ok(departments);
    }

    [HttpGet("{departmentId}")]
    public async Task<ActionResult<DepartmentDetailsDTO>> GetWithDetails([FromRoute] int departmentId)
    {
        var department = await _sender.Send(new GetDepartmentWithDetailsQuery(departmentId));
        return Ok(department);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateDepartmentCommand command)
    {
        var departmentId = await _sender.Send(command);
        return CreatedAtAction(nameof(GetWithDetails), new { departmentId }, command);
    }

    [HttpPut("{departmentId}")]
    public async Task<ActionResult> Update([FromRoute] int departmentId,
                                           [FromBody] UpdateDepartmentCommand command)
    {
        await _sender.Send(command with { Id = departmentId });
        return NoContent();
    }

    [HttpGet("{departmentId}/sections")]
    public async Task<ActionResult<IEnumerable<SectionDTO>>> GetAllSectionsByDepartmentId([FromRoute] int departmentId)
    {
        var sections = await _sender.Send(new GetAllSectionsByDepartmentIdQuery(departmentId));
        return Ok(sections);
    }
}
