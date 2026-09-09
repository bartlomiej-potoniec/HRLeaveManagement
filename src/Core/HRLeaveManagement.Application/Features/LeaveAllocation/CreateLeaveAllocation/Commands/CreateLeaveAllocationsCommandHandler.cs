using LeaveAllocationEntity = HRLeaveManagement.Domain.Leave.LeaveAllocation.LeaveAllocation;
using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Persistence.ContextFactories;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Exceptions;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.CreateLeaveAllocation.Commands;

public sealed class CreateLeaveAllocationsCommandHandler(ILeaveAllocationRepository leaveAllocationRepository,
                                                         ILeaveTypeRepository leaveTypeRepository,
                                                         IEmployeeRepository employeeRepository,
                                                         IEmployeeContextFactory employeeContextFactory,
                                                         LeavePolicyFactory policyFactory,
                                                         TimeProvider timeProvider,
                                                         IAppLogger<CreateLeaveAllocationsCommand> logger) 
    : IRequestHandler<CreateLeaveAllocationsCommand>
{
    private readonly ILeaveAllocationRepository _leaveAllocationRepository = leaveAllocationRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository = leaveTypeRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IEmployeeContextFactory _employeeContextFactory = employeeContextFactory;
    private readonly LeavePolicyFactory _policyFactory = policyFactory;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly IAppLogger<CreateLeaveAllocationsCommand> _logger = logger;

    public async Task Handle(CreateLeaveAllocationsCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveAllocationsCommandValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(CreateLeaveAllocationsCommand));
            throw new BadRequestException("Invalid leave allocations creation request", validationResult);
        }

        var employees = await _employeeRepository.GetAllWithDetailsAsync(cancellationToken);
        var leaveType = await _leaveTypeRepository
            .GetByIdAsync(request.LeaveTypeId, cancellationToken)
            ?? throw new NotFoundException($"No leave type with ID: { request.LeaveTypeId } found");

        List<LeaveAllocationEntity> allocations = [];

        var currentYear = _timeProvider.GetUtcNow().Year;
        var policy = _policyFactory.Create(leaveType);

        foreach (var employee in employees)
        {
            var employeeWithAllInfo = _employeeContextFactory.AsEmployeeWithAllInfo(employee);
            var leaveEvaluationContext = new LeaveEvaluationContext(employeeWithAllInfo);

            if (policy.IsEligible(leaveEvaluationContext))
            {
                var availableDays = policy.CalculateDays(leaveEvaluationContext);
                var allocation = LeaveAllocationEntity.Create(policy, employeeWithAllInfo, leaveType, currentYear, availableDays);

                allocations.Add(allocation);
            }
        }

        _logger.LogInformation("Creating new leave allocations for leave type with ID: {Id} started", request.LeaveTypeId);
        await _leaveAllocationRepository.CreateRangeAsync(allocations, cancellationToken);
        _logger.LogInformation("Creating new leave allocations for leave type with ID: {Id} successful", request.LeaveTypeId);
    }
}
