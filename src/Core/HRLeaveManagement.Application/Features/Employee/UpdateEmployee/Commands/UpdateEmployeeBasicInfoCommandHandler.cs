using DomainEmployee = HRLeaveManagement.Domain.Employee.Employee;
using DomainSection = HRLeaveManagement.Domain.Department.Section.Section;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Persistence.ContextFactories;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Validation;
using MediatR;
using HRLeaveManagement.Application.Features.Employee.CreateEmployee.Commands;

namespace HRLeaveManagement.Application.Features.Employee.UpdateEmployee.Commands;

public sealed class UpdateEmployeeBasicInfoCommandHandler(IEmployeeRepository employeeRepository,
                                                          ISectionRepository sectionRepository,
                                                          IEmployeeContextFactory employeeContextFactory,
                                                          IUserService userService,
                                                          IUnitOfWork unitOfWork,
                                                          IAppLogger<UpdateEmployeeBasicInfoCommandHandler> logger) 
    : IRequestHandler<UpdateEmployeeBasicInfoCommand>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly ISectionRepository _sectionRepository = sectionRepository;
    private readonly IEmployeeContextFactory _employeeContextFactory = employeeContextFactory;
    private readonly IUserService _userService = userService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAppLogger<UpdateEmployeeBasicInfoCommandHandler> _logger = logger;

    public async Task Handle(UpdateEmployeeBasicInfoCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateEmployeeBasicInfoCommandValidator(
            _employeeRepository,
            _sectionRepository,
            _userService
        );

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(CreateEmployeeWithDetailsCommand));
            throw new BadRequestException("Invalid employee update request", validationResult);
        }

        var employee =  await _employeeRepository
            .GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"No user with ID: { request.Id } found");

        DomainEmployee? leader = null;
        DomainSection? section = null;

        if (request.LeaderId is not null)
        {
            leader = await _employeeRepository
                .GetByIdAsync(request.LeaderId.Value, cancellationToken)
                ?? throw new NotFoundException($"No leader with ID: { request.LeaderId } found");
        }

        if (request.SectionId is not null)
        {
            section = await _sectionRepository
                .GetByIdAsync(request.SectionId.Value, cancellationToken)
                ?? throw new NotFoundException($"No section with ID: {request.LeaderId} found");
        }

        var employeeWithAddress = _employeeContextFactory.AsEmployeeWithAddress(employee);

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

        _logger.LogInformation("Updating informations about employee with ID: {UserId} started", request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Updating informations about employee with ID: {UserId} successful", request.Id);
    }
}
