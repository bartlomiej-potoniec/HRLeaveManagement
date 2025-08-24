using LeaveAllocationEntity = HRLeaveManagement.Domain.Entities.LeaveAllocation;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Persistence.ContextFactories;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Exceptions;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.CommandHandlers;

public sealed class CreateLeaveAllocationsCommandHandler(ILeaveAllocationRepository leaveAllocationRepository,
                                                         ILeaveTypeRepository leaveTypeRepository,
                                                         IEmployeeRepository employeeRepository,
                                                         IEmployeeContextFactory employeeContextFactory,
                                                         IAppLogger<CreateLeaveAllocationsCommand> logger) 
    : IRequestHandler<CreateLeaveAllocationsCommand>
{
    private readonly ILeaveAllocationRepository _leaveAllocationRepository = leaveAllocationRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository = leaveTypeRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IEmployeeContextFactory _employeeContextFactory = employeeContextFactory;
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

        var employee = await _employeeRepository
            .GetWithLeaveAllocationsByIdAsync(request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException($"No employee with ID: { request.EmployeeId } found");

        var employeeWithLeaveAllocations = _employeeContextFactory.AsEmployeeWithLeaveAllocations(employee);

        foreach (var allocation in request.LeaveAllocations)
        {
            var leaveType = await _leaveTypeRepository
                .GetByIdAsync(allocation.LeaveTypeId, cancellationToken)
                ?? throw new NotFoundException($"No leave type with ID: { allocation.LeaveTypeId } found");

            var leaveAllocation = LeaveAllocationEntity
                .Create(employeeWithLeaveAllocations, leaveType, request.Year, allocation.AvailableDays);
            
            employeeWithLeaveAllocations.AddLeaveAllocation(leaveAllocation);
        }

        _logger.LogInformation("Creating new leave allocations for employee with ID: {Id} started", request.EmployeeId);

        await _employeeRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Creating new leave allocations for employee with ID: {Id} successful", request.EmployeeId);
    }
}
