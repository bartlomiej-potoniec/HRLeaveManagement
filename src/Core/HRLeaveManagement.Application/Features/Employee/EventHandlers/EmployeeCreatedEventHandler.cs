using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Features.Employee.Events;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.EventHandlers;

public sealed class EmployeeCreatedEventHandler(IUserService userService,
                                                IEmailService emailService,
                                                IAppLogger<EmployeeCreatedEventHandler> logger)
    : INotificationHandler<EmployeeCreatedEvent>
{
    private readonly IUserService _userService = userService;
    private readonly IEmailService _emailService = emailService;
    private readonly IAppLogger<EmployeeCreatedEventHandler> _logger = logger;

    public async Task Handle(EmployeeCreatedEvent notification, CancellationToken cancellationToken)
    {
        var employeeId = notification.Payload.Employee.Id;
        var userId = Guid.Empty; // ?
        var userEmail = ""; // ?
        var userFirstName = ""; // ?



        _logger.LogInformation("Updating new employee with ID: {EmployeeId} with user ID: {UserId} successful", employeeId, userId);

        await _emailService.SendEmployeeCreationEmailAsync(userEmail, userFirstName, cancellationToken);

    }
}
