using DomainSection = HRLeaveManagement.Domain.Entities.Section;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.Section.Commands;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Validation;
using MediatR;

namespace HRLeaveManagement.Application.Features.Section.CommandHandlers;

public sealed class CreateSectionCommandHandler(ISectionRepository sectionRepository,
                                                IDepartmentRepository departmentRepository,
                                                IUserService userService,
                                                IAppLogger<CreateSectionCommandHandler> logger)
    : IRequestHandler<CreateSectionCommand, int>
{
    private readonly ISectionRepository _sectionRepository = sectionRepository;
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;
    private readonly IUserService _userService = userService;
    private readonly IAppLogger<CreateSectionCommandHandler> _logger = logger;

    public async Task<int> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateSectionCommandValidator(_userService, _departmentRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid) 
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(CreateSectionCommand));
            throw new BadRequestException("Invalid section creation request", validationResult);
        }

        var section = DomainSection.Create(
            request.Name,
            request.DepartmentId,
            request.LeaderId,
            request.Description
        );

        _logger.LogInformation("Creating new section '{Name}' started", request.Name);

        await _sectionRepository.CreateAsync(section, cancellationToken);

        _logger.LogInformation("Creating new section '{Name}' successful", request.Name);

        return section.Id;
    }
}
