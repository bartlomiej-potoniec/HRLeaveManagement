using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.ApproveLeaveRequest.Events;

public sealed class LeaveRequestApprovedEventHandler(IEmailService emailService,
                                                     ILeaveAllocationRepository leaveAllocationRepository,
                                                     IUnitOfWork unitOfWork,
                                                     TimeProvider timeProvider,
                                                     IAppLogger<LeaveRequestApprovedEventHandler> logger)
    : INotificationHandler<LeaveRequestApprovedEvent>
{
    private readonly IEmailService _emailService = emailService;
    private readonly ILeaveAllocationRepository _leaveAllocationRepository = leaveAllocationRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly IAppLogger<LeaveRequestApprovedEventHandler> _logger = logger;

    public async Task Handle(LeaveRequestApprovedEvent notification, CancellationToken cancellationToken)
    {
        var requestingUserEmail = notification.AuthMetadata.RequestingUserEmail;
        var requestingUserName = notification.AuthMetadata.RequestingUserName;

        var leaveRequesterId = notification.Payload.LeaveRequesterId;
        var leaveRequestId = notification.Payload.LeaveRequestId;
        var leaveRequestTotalDays = notification.Payload.LeaveRequestTotalDays;
        var leaveRequestCreatedAt = notification.Payload.LeaveRequestCreatedAt;
        var leaveTypeName = notification.Payload.LeaveTypeName;

        var currentYear = _timeProvider.GetUtcNow().Year;

        var allocation = await _leaveAllocationRepository
            .GetUserLeaveAllocationByIdAsync(leaveRequesterId, leaveRequestId, currentYear, cancellationToken)
            ?? throw new NotFoundException($"No leave allocation found");

        allocation.UseDays(leaveRequestTotalDays);

        _logger.LogInformation("Using {UsedDays} days for leave allocation with ID: {Allocation} started", leaveRequestTotalDays, allocation.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Using {UsedDays} days for leave allocation with ID: {Allocation} successfull", leaveRequestTotalDays, allocation.Id);

        _logger.LogInformation("Sending leave request-approval email to user with email: {UserEmail}", requestingUserEmail);
        await _emailService.SendLeaveRequestApprovalEmailAsync(
            requestingUserEmail,
            requestingUserName,
            leaveTypeName,
            leaveRequestCreatedAt,
            cancellationToken
        );
    }
}
