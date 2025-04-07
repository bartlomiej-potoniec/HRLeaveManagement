using HRLeaveManagement.Application.Features.Employee.Commands;
using HRLeaveManagement.Application.Features.Employee.Queries;
using HRLeaveManagement.Application.Features.RemoteWorkLimit.Queries;
using HRLeaveManagement.Application.Features.LeaveAllocation.Queries;
using HRLeaveManagement.Application.DTOs.Employees;
using HRLeaveManagement.Application.DTOs.RemoteWorkLimits;
using HRLeaveManagement.Application.DTOs.LeaveAllocations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace HRLeaveManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "HR")]
public sealed class EmployeesController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var employees = await _sender.Send(new GetAllEmployeesQuery(), cancellationToken);
        return Ok(employees);
    }

    [HttpGet("{employeeId}")]
    //[Authorize(Roles = "HR", Policy = "IsEmployee")]
    //[ValidateGuid]
    public async Task<ActionResult<EmployeeDetailsDTO>> GetWithDetails([FromRoute] Guid employeeId,
                                                                       CancellationToken cancellationToken)
    {
        var employee = await _sender.Send(new GetEmployeeWithDetailsQuery(employeeId), cancellationToken);
        return Ok(employee);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDetailsDTO>> CreateWithDetails([FromBody] CreateEmployeeWithDetailsCommand command,
                                                                          CancellationToken cancellationToken)
    {
        var employeeId = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetWithDetails), new { employeeId }, command);
    }
    
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateBasicInfo([FromRoute] Guid id,
                                                    [FromBody] UpdateEmployeeBasicInfoCommand command,
                                                    CancellationToken cancellationToken)
    {
        await _sender.Send(command with { Id = id }, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateWithDetails([FromRoute] Guid id,
                                                      [FromBody] UpdateEmployeeWithDetailsCommand command,
                                                      CancellationToken cancellationToken)
    {
        await _sender.Send(command with { Id = id }, cancellationToken);
        return NoContent();
    }

    // Contract subentity
    
    [HttpGet("{employeeId}/contracts")]
    public async Task<ActionResult<IEnumerable<EmployeeContractDetailsDTO>>> GetAllContracts([FromRoute] Guid employeeId,
                                                                                             CancellationToken cancellationToken)
    {
        var contracts = await _sender.Send(new GetAllEmployeeContractsQuery(employeeId), cancellationToken);
        return Ok(contracts);
    }
    
    [HttpGet("{employeeId}/contracts/{contractId}")]
    //[Authorize(Roles = "HR", Policy = "IsEmployeeContract")]
    public async Task<ActionResult<EmployeeContractDetailsDTO>> GetContractWithDetails([FromRoute] Guid employeeId,
                                                                                       [FromRoute] int contractId,
                                                                                       CancellationToken cancellationToken)
    {
        var contract = await _sender.Send(
            new GetEmployeeContractWithDetailsCommand(employeeId, contractId), 
            cancellationToken
        );

        return Ok(contract);
    }
    
    [HttpPost("{employeeId}/contracts/")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeContractDetailsDTO>> CreateContract([FromRoute] Guid employeeId,
                                                                               [FromBody] CreateEmployeeContractCommand command,
                                                                               CancellationToken cancellationToken)
    {
        var contractId = await _sender.Send(command with { EmployeeId = employeeId }, cancellationToken);
        return CreatedAtAction(nameof(GetContractWithDetails), new { employeeId, contractId }, command);
    }

    [HttpPut("{employeeId}/contracts/{contractId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateContract([FromRoute] Guid employeeId,
                                                   [FromRoute] int contractId,
                                                   [FromBody] UpdateEmployeeContractCommand command,
                                                   CancellationToken cancellationToken)
    {
        await _sender.Send(
            command with { EmployeeId = employeeId, ContractId = contractId },
            cancellationToken
        );

        return NoContent();
    }

    [HttpPatch("{employeeId}/contracts/{contractId}/terminate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> TerminateContract([FromRoute] Guid employeeId,
                                                      [FromRoute] int contractId,
                                                      [FromBody] TerminateEmployeeContractCommand command,
                                                      CancellationToken cancellationToken)
    {
        await _sender.Send(
            command with { EmployeeId = employeeId, ContractId = contractId },
            cancellationToken
        );

        return NoContent();
    }

    /*
    // Education subentity

    [HttpGet("{employeeId}/educations")]
    public async Task<ActionResult<IEnumerable<EmployeeEducationDTO>>> GetAllEducations([FromRoute] Guid employeeId)
    {
        var educations = await _sender.Send(new GetAllEmployeeEducationsCommand(employeeId));
        return Ok(educations);
    }

    [HttpGet("{employeeId}/educations/{educationId}")]
    //[Authorize(Roles = "HR", Policy = "IsEmployee")]
    public async Task<ActionResult<EmployeeEducationDetailsDTO>> GetEducationWithDetails([FromRoute] Guid employeeId,
                                                                                         [FromRoute] int educationId)
    {
        var education = await _sender.Send(new GetEmployeeEducationWithDetailsCommand(employeeId, educationId));
        return Ok(education);
    }

    [HttpPost("{employeeId}/educations/")]
    public async Task<ActionResult> CreateEducation([FromRoute] Guid employeeId,
                                                    [FromBody] CreateEmployeeEducationCommand command)
    {
        var education  = await _sender.Send(command with { EmployeeId = employeeId });
        return Ok(education);
    }

    [HttpPatch("{employeeId}/educations/{educationId}")]
    public async Task<ActionResult> UpdateEducation([FromRoute] Guid employeeId,
                                                    [FromRoute] Guid educationId,
                                                    [FromBody] UpdateEmployeeEducationCommand command)
    {
        await _sender.Send(command with { EmployeeId = employeeId, EducationId = educationId });
        return NoContent();
    }

    // Experience subentity

    [HttpGet("{employeeId}/experiences")]
    public async Task<ActionResult<IEnumerable<EmployeeExperienceDTO>>> GetAllExperiences([FromRoute] Guid employeeId)
    {
        var experiences = await _sender.Send(new GetAllEmployeeExperiencesCommand(employeeId));
        return Ok(experiences);
    }

    [HttpGet("{employeeId}/experiences/{experienceId}")]
    //[Authorize(Roles = "HR", Policy = "IsEmployee")]
    public async Task<ActionResult<EmployeeExperienceDetailsDTO>> GetExperienceWithDetails([FromRoute] Guid employeeId,
                                                                                           [FromRoute] int experienceId)
    {
        var experience = await _sender.Send(new GetEmployeeExperienceWithDetailsCommand(employeeId, experienceId));
        return Ok(experience);
    }

    [HttpPost("{employeeId}/experiences/")]
    public async Task<ActionResult> CreateExperience([FromRoute] Guid employeeId,
                                                     [FromBody] CreateEmployeeExperienceCommand command)
    {
        var experience = await _sender.Send(command with { EmployeeId = employeeId });
        return Ok(experience);
    }


    [HttpPatch("{employeeId}/experiences/{experienceId}")]
    public async Task<ActionResult> UpdateExperience([FromRoute] Guid employeeId,
                                                     [FromRoute] Guid experienceId,
                                                     [FromBody] UpdateEmployeeExperienceCommand command)
    {
        await _sender.Send(command with { EmployeeId = employeeId, ExperienceId = experienceId });
        return NoContent();
    }
    */

    // RemoteWorkLimit subentity

    [HttpGet("{employeeId}/remoteWorkLimits")]
    public async Task<ActionResult<IEnumerable<RemoteWorkLimitDTO>>> GetAllRemoteWorkLimitsForEmployee([FromRoute] Guid employeeId,
                                                                                                       CancellationToken cancellationToken)
    {
        var remoteWorkLimits = await _sender.Send(
            new GetAllRemoteWorkLimitsForEmployeeQuery(employeeId),
            cancellationToken
        );

        return Ok(remoteWorkLimits);
    }

    [HttpGet("{employeeId}/leaveAllocations")]
    public async Task<ActionResult<IEnumerable<LeaveAllocationDetailsDTO>>> GetAllLeaveAllocationsForEmployee([FromRoute] Guid employeeId,
                                                                                                              CancellationToken cancellationToken)
    {
        var leaveAllocations = await _sender.Send(
            new GetAllLeaveAllocationsForEmployeeQuery(employeeId),
            cancellationToken
        );

        return Ok(leaveAllocations);
    }
}
