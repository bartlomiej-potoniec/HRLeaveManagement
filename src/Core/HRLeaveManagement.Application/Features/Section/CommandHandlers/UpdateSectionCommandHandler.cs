using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Persistence.ContextFactories;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Features.Section.Commands;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Validation;
using MediatR;

namespace HRLeaveManagement.Application.Features.Section.CommandHandlers;

public sealed class UpdateSectionCommandHandler(ISectionRepository sectionRepository,
                                                IEmployeeRepository employeeRepository,
                                                IDepartmentRepository departmentRepository,
                                                IDepartmentContextFactory departmentContextFactory,
                                                IUserService userService,
                                                IAppLogger<UpdateSectionCommandHandler> logger) 
    : IRequestHandler<UpdateSectionCommand>
{
    private readonly ISectionRepository _sectionRepository = sectionRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;
    private readonly IDepartmentContextFactory _departmentContextFactory = departmentContextFactory;
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

        var leader = await _employeeRepository
            .GetByIdAsync(request.LeaderId, cancellationToken)
            ?? throw new NotFoundException($"No employee with ID: { request.LeaderId } found", cancellationToken);

        var department = await _departmentRepository
            .GetWithDetailsById(request.DepartmentId, cancellationToken)
            ?? throw new NotFoundException($"No department with ID: { request.Id } found");
        
        var departmentWithSections = _departmentContextFactory.AsDepartmentWithSections(department); 
        section.Update(departmentWithSections, request.Name, leader, request.Description);

        _logger.LogInformation("Updating informations about section with ID: {Id} started", request.Id);

        await _sectionRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updating informations about section with ID: {Id} successful", request.Id);
    }
}
