using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Features.LeaveRequest.Events;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.EventHandlers;

public sealed class LeaveRequestRejectedEventHandler(IEmailService emailService,
                                                     IAppLogger<LeaveRequestApprovedEventHandler> logger)
    : INotificationHandler<LeaveRequestRejectedEvent>
{
    private readonly IEmailService _emailService = emailService;
    private readonly IAppLogger<LeaveRequestApprovedEventHandler> _logger = logger;

    public async Task Handle(LeaveRequestRejectedEvent notification, CancellationToken cancellationToken)
    {
        var requestingUserEmail = notification.AuthMetadata.RequestingUserEmail;
        var requestingUserName = notification.AuthMetadata.RequestingUserName;
        var leaveRequestCreatedAt = notification.Payload.LeaveRequestCreatedAt;
        var leaveTypeName = notification.Payload.LeaveTypeName;

        _logger.LogInformation("Sending leave request-rejection email to user with email: {UserEmail}", requestingUserEmail);
        await _emailService.SendLeaveRequestRejectionEmailAsync(
            requestingUserEmail,
            requestingUserName,
            leaveTypeName,
            leaveRequestCreatedAt,
            cancellationToken
        );
    }
}