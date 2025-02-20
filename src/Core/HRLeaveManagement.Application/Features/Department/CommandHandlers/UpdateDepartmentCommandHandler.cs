using DomainDepartment = HRLeaveManagement.Domain.Entities.Department;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.Department.Commands;
using HRLeaveManagement.Application.Validation;
using MediatR;

namespace HRLeaveManagement.Application.Features.Department.CommandHandlers;

public sealed class UpdateDepartmentCommandHandler(IDepartmentRepository departmentRepository,
                                                   IUserService userService,
                                                   IAppLogger<UpdateDepartmentCommandHandler> logger)
    : IRequestHandler<UpdateDepartmentCommand>
{
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;
    private readonly IUserService _userService = userService;
    private readonly IAppLogger<UpdateDepartmentCommandHandler> _logger = logger;

    public async Task Handle(UpdateDepartmentCommand request,
                             CancellationToken cancellationToken)
    {
        var validator = new UpdateDepartmentCommandValidator(_userService, _departmentRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(UpdateDepartmentCommand));
            throw new BadRequestException("Invalid department updating request", validationResult);
        }

        var department = await _departmentRepository
            .GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"No department with ID {request.Id} found");

        DomainDepartment.Update(department, request.Name, request.LeaderId, request.Description);

        _logger.LogInformation("Updating informations about department with ID: {Id} started", request.Id);

        await _departmentRepository.UpdateAsync(department);

        _logger.LogInformation("Updating informations about department with ID: {Id} successful", request.Id);
    }
}
