using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Features.LeaveRequest.Events;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.EventHandlers;

public sealed class LeaveRequestCreatedEventHandler(IEmailService emailService, IAppLogger<LeaveRequestCreatedEventHandler> logger)
    : INotificationHandler<LeaveRequestCreatedEvent>
{
    private readonly IEmailService _emailService = emailService;
    private readonly IAppLogger<LeaveRequestCreatedEventHandler> _logger = logger;

    public Task Handle(LeaveRequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        
    }
}
