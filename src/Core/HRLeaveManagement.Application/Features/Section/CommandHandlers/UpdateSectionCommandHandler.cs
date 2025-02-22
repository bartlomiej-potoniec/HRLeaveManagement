using DomainSection = HRLeaveManagement.Domain.Entities.Section;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.Section.Commands;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Validation;
using MediatR;

namespace HRLeaveManagement.Application.Features.Section.CommandHandlers;

public sealed class UpdateSectionCommandHandler(ISectionRepository sectionRepository,
                                                IDepartmentRepository departmentRepository,
                                                IUserService userService,
                                                IAppLogger<UpdateSectionCommandHandler> logger) 
    : IRequestHandler<UpdateSectionCommand>
{
    private readonly ISectionRepository _sectionRepository = sectionRepository;
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;
    private readonly IUserService _userService = userService;
    private readonly IAppLogger<UpdateSectionCommandHandler> _logger = logger;

    public async Task Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateSectionCommandValidator(
            _userService,
            _sectionRepository,
            _departmentRepository
        );

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(UpdateSectionCommand));
            throw new BadRequestException("Invalid section updating request", validationResult);
        }

        var section = await _sectionRepository
            .GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"No section with ID: { request.Id } found");

        DomainSection.Update(
            section,
            request.Name,
            request.DepartmentId,
            request.LeaderId,
            request.Description
        );

        _logger.LogInformation("Updating informations about section with ID: {Id} started", request.Id);

        await _sectionRepository.UpdateAsync(section, cancellationToken);

        _logger.LogInformation("Updating informations about section with ID: {Id} successful", request.Id);
    }
}
