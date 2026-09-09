using DomainLeaveAllocation = HRLeaveManagement.Domain.Leave.LeaveAllocation.LeaveAllocation;
using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Persistence.ContextFactories;
using HRLeaveManagement.Application.Features.LeaveAllocation;
using HRLeaveManagement.Application.Exceptions;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.CreateEmployee.Events;

public sealed class EmployeeCreatedEventHandler(IUserService userService,
                                                IEmailService emailService,
                                                IEmployeeRepository employeeRepository,
                                                ILeaveTypeRepository leaveTypeRepository,
                                                ILeaveAllocationRepository leaveAllocationRepository,
                                                IEmployeeContextFactory employeeContextFactory,
                                                LeavePolicyFactory policyFactory,
                                                TimeProvider timeProvider,
                                                IAppLogger<EmployeeCreatedEventHandler> logger)
    : INotificationHandler<EmployeeCreatedEvent>
{
    private readonly IUserService _userService = userService;
    private readonly IEmailService _emailService = emailService;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository = leaveTypeRepository;
    private readonly ILeaveAllocationRepository _leaveAllocationRepository = leaveAllocationRepository;
    private readonly IEmployeeContextFactory _employeeContextFactory = employeeContextFactory;
    private readonly LeavePolicyFactory _policyFactory = policyFactory;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly IAppLogger<EmployeeCreatedEventHandler> _logger = logger;

    public async Task Handle(EmployeeCreatedEvent notification, CancellationToken cancellationToken)
    {
        var userId = notification.Payload.UserId;
        var employeeId = notification.Payload.EmployeeId;
        var employeeFirstName = notification.Payload.EmployeeFirstName;
        var currentYear = _timeProvider.GetUtcNow().Year;
        List<DomainLeaveAllocation> leaveAllocations = [];

        // Update User Employee-ID with newly created Employee ID
        _logger.LogInformation("Updating new employee with ID: {EmployeeId} with user ID: {UserId}", employeeId, userId);
        var user = await _userService.UpdateUserEmployeeIdAsync(userId, employeeId, cancellationToken);
        _logger.LogInformation("Updating new employee with ID: {EmployeeId} with user ID: {UserId} successful", employeeId, userId);

        // Allocate predefined (and custom) leaves for new employee on current year
        var leaveTypes = await _leaveTypeRepository.GetAllAsync(cancellationToken);
        var employee = await _employeeRepository
            .GetByIdAsync(employeeId, cancellationToken)
            ?? throw new NotFoundException($"No employee with ID: { employeeId }");

        var employeeWithAllInfo = _employeeContextFactory.AsEmployeeWithAllInfo(employee);
        var leaveEvaluationContext = new LeaveEvaluationContext(employeeWithAllInfo);

        foreach (var leaveType in leaveTypes)
        {
            var policy = _policyFactory.Create(leaveType);

            if (policy.IsEligible(leaveEvaluationContext))
            {
                var availableDays = policy.CalculateDays(leaveEvaluationContext);
                var allocation = DomainLeaveAllocation.Create(policy, employeeWithAllInfo, leaveType, currentYear, availableDays);

                leaveAllocations.Add(allocation);
            }
        }

        _logger.LogInformation("Creating leave allocation for employee with ID: {EmployeeId}", employeeId);
        await _leaveAllocationRepository.CreateRangeAsync(leaveAllocations, cancellationToken);

        // Send employee-creation email
        _logger.LogInformation("Sending employee-creation email to user with email: {UserEmail}", user.Email);
        await _emailService.SendEmployeeCreationEmailAsync(user.Email, employeeFirstName, cancellationToken);
    }
}
