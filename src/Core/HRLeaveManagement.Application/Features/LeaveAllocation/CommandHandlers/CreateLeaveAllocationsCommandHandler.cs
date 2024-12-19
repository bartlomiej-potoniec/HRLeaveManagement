using DomainLeaveAllocation = HRLeaveManagement.Domain.Entities.LeaveAllocation;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Exceptions;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.CommandHandlers;

public sealed class CreateLeaveAllocationsCommandHandler(ILeaveAllocationRepository leaveAllocationRepository,
                                                         ILeaveTypeRepository leaveTypeRepository,
                                                         IEmployeeRepository employeeRepository,
                                                         IAppLogger<CreateLeaveAllocationsCommand> logger) 
    : IRequestHandler<CreateLeaveAllocationsCommand>
{
    private readonly ILeaveAllocationRepository _leaveAllocationRepository = leaveAllocationRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository = leaveTypeRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IAppLogger<CreateLeaveAllocationsCommand> _logger = logger;

    public async Task Handle(CreateLeaveAllocationsCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveAllocationsCommandValidator(
            _leaveAllocationRepository,
            _leaveTypeRepository,
            _employeeRepository
        );

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(CreateLeaveAllocationsCommand));
            throw new BadRequestException("Invalid leave allocations creation request", validationResult);
        }

        var leaveAllocations = request.LeaveAllocations
            .Select(la => DomainLeaveAllocation.Create(request.EmployeeId, la.LeaveTypeId, request.Year, la.AvailableDays));

        _logger.LogInformation("Creating new leave allocations for employee with ID: {Id} started", request.EmployeeId);

        await _leaveAllocationRepository.CreateRangeAsync(leaveAllocations);

        _logger.LogInformation("Creating new leave allocations for employee with ID: {Id} successful", request.EmployeeId);
    }
}
