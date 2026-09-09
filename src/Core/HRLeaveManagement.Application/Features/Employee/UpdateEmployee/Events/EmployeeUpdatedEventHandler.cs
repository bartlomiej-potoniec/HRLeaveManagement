using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.UpdateEmployee.Events;

public sealed class EmployeeUpdatedEventHandler(IEmailService emailService,
                                                IAppLogger<EmployeeUpdatedEventHandler> logger)
    : INotificationHandler<EmployeeUpdatedEvent>
{
    private readonly IEmailService _emailService = emailService;
    private readonly IAppLogger<EmployeeUpdatedEventHandler> _logger = logger;

    public async Task Handle(EmployeeUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var requestingUserEmail = notification.AuthMetadata.RequestingUserEmail;
        var employeeId = notification.Payload.EmployeeId;
        var employeeFirstName = notification.Payload.EmployeeFirstName;
        var employeeLastName = notification.Payload.EmployeeLastName;

        _logger.LogInformation("Sending employee-updating email to user with email: {UserEmail}", requestingUserEmail);
        await _emailService.SendEmployeeUpdatingEmailAsync(
            requestingUserEmail,
            employeeId,
            employeeFirstName,
            employeeLastName,
            cancellationToken
        );
    }
}
