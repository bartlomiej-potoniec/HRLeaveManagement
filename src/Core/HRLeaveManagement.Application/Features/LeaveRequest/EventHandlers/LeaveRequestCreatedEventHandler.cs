using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Features.LeaveRequest.Events;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.EventHandlers;

public sealed class LeaveRequestCreatedEventHandler(IEmailService emailService,
                                                    IAppLogger<LeaveRequestCreatedEventHandler> logger)
    : INotificationHandler<LeaveRequestCreatedEvent>
{
    private readonly IEmailService _emailService = emailService;
    private readonly IAppLogger<LeaveRequestCreatedEventHandler> _logger = logger;

    public async Task Handle(LeaveRequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        var requestingUserEmail = notification.AuthMetadata.RequestingUserEmail;
        var requesterFullName = notification.Payload.RequesterFullName;
        var approverFullName = notification.Payload.ApproverFullName;
        var leaveTypeName = notification.Payload.LeaveTypeName;
        var leaveStartedAt = notification.Payload.LeaveStartedAt;
        var leaveEndedAt = notification.Payload.LeaveEndedAt;

        _logger.LogInformation("Sending leave request-creation email to user with email: {UserEmail}", requestingUserEmail);
        await _emailService.SendLeaveRequestCreationEmail(
            requestingUserEmail,
            requesterFullName,
            approverFullName,
            leaveTypeName,
            leaveStartedAt,
            leaveEndedAt,
            cancellationToken
        );
    }
}
