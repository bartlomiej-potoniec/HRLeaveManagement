using DomainEmployee = HRLeaveManagement.Domain.Entities.Employee;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.Employee.Commands;
using HRLeaveManagement.Application.Validation;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.CommandHandlers;

public sealed class UpdateEmployeeBasicInfoCommandHandler(IEmployeeRepository employeeRepository,
                                                          ISectionRepository sectionRepository,
                                                          IUserService userService,
                                                          IAppLogger<UpdateEmployeeBasicInfoCommandHandler> logger) 
    : IRequestHandler<UpdateEmployeeBasicInfoCommand>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly ISectionRepository _sectionRepository = sectionRepository;
    private readonly IUserService _userService = userService;
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

        DomainEmployee.Update(
            employee,
            request.Position,
            request.Responsibilities,
            request.SectionId,
            request.LeaderId
        );

        _logger.LogInformation("Updating informations about employee with ID: {UserId} started", request.Id);

        await _employeeRepository.UpdateBasicInfoAsync(employee, cancellationToken);

        _logger.LogInformation("Updating informations about employee with ID: {UserId} successful", request.Id);
    }
}
