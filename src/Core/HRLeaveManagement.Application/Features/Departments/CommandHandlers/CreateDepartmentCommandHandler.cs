using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.Departments.Commands;
using HRLeaveManagement.Application.Validation;
using MediatR;

namespace HRLeaveManagement.Application.Features.Departments.CommandHandlers;

public sealed class CreateDepartmentCommandHandler(IDepartmentRepository departmentRepository,
                                                   IEmployeeRepository employeeRepository,
                                                   IAppLogger<CreateDepartmentCommandHandler> logger)
    : IRequestHandler<CreateDepartmentCommand, int>
{
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IAppLogger<CreateDepartmentCommandHandler> _logger = logger;

    public async Task<int> Handle(CreateDepartmentCommand request,
                                  CancellationToken cancellationToken)
    {
        var validator = new CreateDepartmentCommandValidator(_employeeRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(CreateDepartmentCommand));
            throw new BadRequestException("Invalid department creation request", validationResult);
        }

        var department = Department.Create(request.Name, request.LeaderId, request.Description);

        _logger.LogInformation("Creating new department '{Name}' started", request.Name);

        await _departmentRepository.CreateAsync(department);

        _logger.LogInformation("Creating new department '{Name}' successful", request.Name);

        return department.Id;
    }
}
