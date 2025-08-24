using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Persistence.ContextFactories;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Features.Section.Commands;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Validation;
using MediatR;

namespace HRLeaveManagement.Application.Features.Section.CommandHandlers;

public sealed class CreateSectionCommandHandler(IDepartmentRepository departmentRepository,
                                                IEmployeeRepository employeeRepository,
                                                IDepartmentContextFactory departmentContextFactory,
                                                IUserService userService,
                                                IAppLogger<CreateSectionCommandHandler> logger)
    : IRequestHandler<CreateSectionCommand, int>
{
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IDepartmentContextFactory _departmentContextFactory = departmentContextFactory;
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

        var leader = await _employeeRepository
            .GetByIdAsync(request.LeaderId, cancellationToken)
            ?? throw new NotFoundException($"No employee with ID: { request.LeaderId } found");

        var department = await _departmentRepository
            .GetWithDetailsById(request.DepartmentId, cancellationToken)
            ?? throw new NotFoundException($"No section with ID: { request.DepartmentId } found");

        var departmentWithSections = _departmentContextFactory.AsDepartmentWithSections(department);
        var section = departmentWithSections.AddSingleSection(request.Name, leader, request.Description);

        _logger.LogInformation("Creating new section '{Name}' started", request.Name);

        await _departmentRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Creating new section '{Name}' successful", request.Name);

        return section.Id;
    }
}
