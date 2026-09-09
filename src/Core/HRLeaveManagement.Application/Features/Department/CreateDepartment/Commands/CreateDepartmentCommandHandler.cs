using DepartmentEntity = HRLeaveManagement.Domain.Department.Department;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Validation;
using MediatR;

namespace HRLeaveManagement.Application.Features.Department.CreateDepartment.Commands;

public sealed class CreateDepartmentCommandHandler(IDepartmentRepository departmentRepository,
                                                   IEmployeeRepository employeeRepository,
                                                   IDepartmentRuleSet departmentRuleSet,
                                                   IUserService userService,
                                                   IAppLogger<CreateDepartmentCommandHandler> logger)
    : IRequestHandler<CreateDepartmentCommand, int>
{
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IDepartmentRuleSet _departmentRuleSet = departmentRuleSet;
    private readonly IUserService _userService = userService;
    private readonly IAppLogger<CreateDepartmentCommandHandler> _logger = logger;

    public async Task<int> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateDepartmentCommandValidator(_userService);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(CreateDepartmentCommand));
            throw new BadRequestException("Invalid department creation request", validationResult);
        }

        var leader = await _employeeRepository.GetByIdAsync(request.LeaderId, cancellationToken)
            ?? throw new NotFoundException($"No employee with ID: {request.LeaderId} found");

        var department = await DepartmentEntity
            .CreateSingleAsync(_departmentRuleSet, request.Name, leader, request.Description, cancellationToken);

        _logger.LogInformation("Creating new department '{Name}' started", request.Name);
        await _departmentRepository.CreateAsync(department, cancellationToken);
        _logger.LogInformation("Creating new department '{Name}' successful", request.Name);

        return department.Id;
    }
}
