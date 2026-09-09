using HRLeaveManagement.Application.DTOs.Sections;
using HRLeaveManagement.Application.Features.Section.Queries;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using HRLeaveManagement.Application.Features.Department.Section.CreateSection.Commands;
using HRLeaveManagement.Application.Features.Department.Section.UpdateSection.Commands;

namespace HRLeaveManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class SectionsController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SectionDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var sections = await _sender.Send(new GetAllSectionsQuery(), cancellationToken);
        return Ok(sections);
    }

    [HttpGet("{sectionId}")]
    public async Task<ActionResult<SectionDetailsDTO>> GetWithDetails([FromRoute] int sectionId,
                                                                      CancellationToken cancellationToken)
    {
        var section = await _sender.Send(new GetSectionWithDetailsQuery(sectionId), cancellationToken);
        return Ok(section);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateSectionCommand command,
                                                CancellationToken cancellationToken)
    {
        var sectionId = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetWithDetails), new { sectionId }, command);
    }

    [HttpPut("{sectionId}")]
    public async Task<ActionResult> Update([FromRoute] int sectionId,
                                           [FromBody] UpdateSectionCommand command,
                                           CancellationToken cancellationToken)
    {
        await _sender.Send(command with { Id = sectionId }, cancellationToken);
        return NoContent();
    }
}
