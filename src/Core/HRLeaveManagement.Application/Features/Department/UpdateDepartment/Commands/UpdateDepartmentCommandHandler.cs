using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Contracts.Persistence;
using MediatR;
using HRLeaveManagement.Domain.Department.Department;

namespace HRLeaveManagement.Application.Features.Department.UpdateDepartment.Commands;

public sealed class UpdateDepartmentCommandHandler(IDepartmentRepository departmentRepository,
                                                   IEmployeeRepository employeeRepository,
                                                   IDepartmentRuleSet departmentRuleSet,
                                                   IUserService userService,
                                                   IUnitOfWork unitOfWork,
                                                   IAppLogger<UpdateDepartmentCommandHandler> logger)
    : IRequestHandler<UpdateDepartmentCommand>
{
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IDepartmentRuleSet _departmentRuleSet = departmentRuleSet;
    private readonly IUserService _userService = userService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAppLogger<UpdateDepartmentCommandHandler> _logger = logger;

    public async Task Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateDepartmentCommandValidator(_userService);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(UpdateDepartmentCommand));
            throw new BadRequestException("Invalid department updating request", validationResult);
        }

        var department = await _departmentRepository
            .GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"No department with ID { request.Id } found");

        var leader = await _employeeRepository
            .GetByIdAsync(request.LeaderId, cancellationToken)
            ?? throw new NotFoundException($"No employee with ID: { request.LeaderId } found");

        await department.UpdateAsync(_departmentRuleSet, request.Name, leader, request.Description, cancellationToken);

        _logger.LogInformation("Updating informations about department with ID: {Id} started", request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Updating informations about department with ID: {Id} successful", request.Id);
    }
}
