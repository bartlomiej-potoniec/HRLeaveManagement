using HRLeaveManagement.Application.DTOs.Sections;
using HRLeaveManagement.Application.Features.Section.Queries;
using HRLeaveManagement.Application.Features.Section.Commands;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace HRLeaveManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class SectionsController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SectionDTO>>> GetAll()
    {
        var sections = await _sender.Send(new GetAllSectionsQuery());
        return Ok(sections);
    }

    [HttpGet("{sectionId}")]
    public async Task<ActionResult<SectionDetailsDTO>> GetWithDetails([FromRoute] int sectionId)
    {
        var section = await _sender.Send(new GetSectionWithDetailsQuery(sectionId));
        return Ok(section);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateSectionCommand command)
    {
        var sectionId = await _sender.Send(command);
        return CreatedAtAction(nameof(GetWithDetails), new { sectionId }, command);
    }

    [HttpPut("{sectionId}")]
    public async Task<ActionResult> Update([FromRoute] int sectionId,
                                           [FromBody] UpdateSectionCommand command)
    {
        await _sender.Send(command with { Id = sectionId });
        return NoContent();
    }
}
