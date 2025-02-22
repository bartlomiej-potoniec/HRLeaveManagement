using DomainDepartment = HRLeaveManagement.Domain.Entities.Department;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.Department.Commands;
using HRLeaveManagement.Application.Validation;
using MediatR;
using HRLeaveManagement.Application.Contracts.Identity;

namespace HRLeaveManagement.Application.Features.Department.CommandHandlers;

public sealed class CreateDepartmentCommandHandler(IDepartmentRepository departmentRepository,
                                                   IUserService userService,
                                                   IAppLogger<CreateDepartmentCommandHandler> logger)
    : IRequestHandler<CreateDepartmentCommand, int>
{
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;
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

        var department = DomainDepartment.Create(request.Name, request.LeaderId, request.Description);

        _logger.LogInformation("Creating new department '{Name}' started", request.Name);

        await _departmentRepository.CreateAsync(department, cancellationToken);

        _logger.LogInformation("Creating new department '{Name}' successful", request.Name);

        return department.Id;
    }
}
