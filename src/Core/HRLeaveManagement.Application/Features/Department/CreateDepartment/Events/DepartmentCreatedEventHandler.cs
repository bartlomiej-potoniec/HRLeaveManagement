using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using MediatR;

namespace HRLeaveManagement.Application.Features.Department.CreateDepartment.Events;

public sealed class DepartmentCreatedEventHandler(IEmailService emailService,
                                                  IAppLogger<DepartmentCreatedEventHandler> logger) 
    : INotificationHandler<DepartmentCreatedEvent>
{
    private readonly IEmailService _emailService = emailService;
    private readonly IAppLogger<DepartmentCreatedEventHandler> _logger = logger;

    public async Task Handle(DepartmentCreatedEvent notification, CancellationToken cancellationToken)
    {
        var requestingUserEmail = notification.AuthMetadata.RequestingUserEmail;
        var departmentId = notification.Payload.DepartmentId;
        var departmentName = notification.Payload.DepartmentName;

        _logger.LogInformation("Sending department-creation email to user with email: {UserEmail}", requestingUserEmail);
        await _emailService.SendDepartmentCreationEmailAsync(
            requestingUserEmail,
            departmentId,
            departmentName,
            cancellationToken
        );
    }
}
