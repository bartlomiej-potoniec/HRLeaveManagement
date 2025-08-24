using DomainEmployee = HRLeaveManagement.Domain.Entities.Employee;
using DomainSection = HRLeaveManagement.Domain.Entities.Section;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Application;
using HRLeaveManagement.Application.Features.Employee.Commands;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Exceptions;
using MediatR;
using HRLeaveManagement.Application.Contracts.Persistence.ContextFactories;

namespace HRLeaveManagement.Application.Features.Employee.CommandHandlers;

public sealed class UpdateEmployeeWithDetailsCommandHandler(IEmployeeRepository employeeRepository,
                                                            ISectionRepository sectionRepository,
                                                            IEmployeeContextFactory employeeContextFactory,
                                                            IEmployeeSubservice employeeSubservice,
                                                            IUserService userService,
                                                            IAppLogger<UpdateEmployeeWithDetailsCommandHandler> logger) 
    : IRequestHandler<UpdateEmployeeWithDetailsCommand>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly ISectionRepository _sectionRepository = sectionRepository;
    private readonly IEmployeeContextFactory _employeeContextFactory = employeeContextFactory;
    private readonly IEmployeeSubservice _employeeSubservice = employeeSubservice;
    private readonly IUserService _userService = userService;
    private readonly IAppLogger<UpdateEmployeeWithDetailsCommandHandler> _logger = logger;

    public async Task Handle(UpdateEmployeeWithDetailsCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateEmployeeWithDetailsCommandValidator(
            _employeeRepository,
            _sectionRepository,
            _userService
        );

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(CreateEmployeeWithDetailsCommand));
            throw new BadRequestException("Invalid employee with details update request", validationResult);
        }

        var employeeWithDetails = await _employeeRepository
            .GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"No employee with ID: { request.Id } found");

        DomainEmployee? leader = default;
        DomainSection? section = default;

        if (request.LeaderId is not null)
        {
            leader = await _employeeRepository
                .GetByIdAsync(request.LeaderId.Value, cancellationToken)
                ?? throw new NotFoundException($"No employee with ID: {request.LeaderId} found");
        }

        if (request.SectionId is not null)
        {
            section = await _sectionRepository
                .GetByIdAsync(request.SectionId.Value, cancellationToken)
                ?? throw new NotFoundException($"No section with ID: {request.SectionId} found");
        }

        var employeeWithAddress = _employeeContextFactory.AsEmployeeWithAddress(employeeWithDetails);

        employeeWithAddress.Update(
            request.Position,
            request.Responsibilities,
            request.ResidentialAddress,
            request.RegisteredAddress,
            request.SecondaryResidentialAddress,
            request.RemoteWorkAddress,
            section,
            leader
        );

        await _employeeSubservice.UpdateEmployeeContracts(employeeWithDetails, request.EmployeeContracts, cancellationToken);
        await _employeeSubservice.UpdateEmployeeEducations(employeeWithDetails, request.EmployeeEducations, cancellationToken);
        await _employeeSubservice.UpdateEmployeeExperiences(employeeWithDetails, request.EmployeeExperiences, cancellationToken);

        _logger.LogInformation("Updating employee with ID: {EmployeeId} started", request.Id);

        await _employeeRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updating employee with ID: {EmployeeId} successful", request.Id);
    }
}
