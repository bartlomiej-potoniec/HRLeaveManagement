using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.UpdateLeaveAllocation.Events;

public class LeaveAllocationUpdatedEventHandler(IEmailService emailService,
                                                IAppLogger<LeaveAllocationUpdatedEventHandler> logger)
    : INotificationHandler<LeaveAllocationUpdatedEvent>
{
    private readonly IEmailService _emailService = emailService;
    private readonly IAppLogger<LeaveAllocationUpdatedEventHandler> _logger = logger;

    public async Task Handle(LeaveAllocationUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var requestingUserEmail = notification.AuthMetadata.RequestingUserEmail;
        var employeeId = notification.Payload.EmployeeId;
        var employeeFirstName = notification.Payload.EmployeeFirstName;
        var employeeLastName = notification.Payload.EmployeeLastName;
        var availableDays = notification.Payload.AvailableDays;

        // Send employee-creation email
        _logger.LogInformation("Sending leave allocation-updating email to user with email: {UserEmail}", requestingUserEmail);
        await _emailService.SendLeaveAllocationUdpatingEmailAsync(
            requestingUserEmail,
            employeeId,
            employeeFirstName,
            employeeLastName,
            availableDays,
            cancellationToken
        );
    }
}
