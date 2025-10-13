using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveRequest.Events;
using HRLeaveManagement.Domain.Enums;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.EventHandlers;

public sealed class LeaveRequestCanceledEventHandler(IEmailService emailService,
                                                     ILeaveAllocationRepository leaveAllocationRepository,
                                                     IUnitOfWork unitOfWork,
                                                     TimeProvider timeProvider,
                                                     IAppLogger<LeaveRequestCanceledEventHandler> logger)
    : INotificationHandler<LeaveRequestCanceledEvent>
{
    private readonly IEmailService _emailService = emailService;
    private readonly ILeaveAllocationRepository _leaveAllocationRepository = leaveAllocationRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly IAppLogger<LeaveRequestCanceledEventHandler> _logger = logger;

    public async Task Handle(LeaveRequestCanceledEvent notification, CancellationToken cancellationToken)
    {
        var requestingUserName = notification.AuthMetadata.RequestingUserName;
        var requestingUserEmail = notification.AuthMetadata.RequestingUserName;
        
        var leaveRequesterId = notification.Payload.RequesterId;
        var leaveRequestId = notification.Payload.LeaveRequestId;
        var leaveRequestStatus = notification.Payload.RequestStatus;
        var leaveRequestCreatedAt = notification.Payload.LeaveRequestCreatedAt;
        var leaveTypeName = notification.Payload.LeaveTypeName;

        var currentYear = _timeProvider.GetUtcNow().Year; 

        if (leaveRequestStatus is RequestStatus.Approved)
        {
            var allocation = await _leaveAllocationRepository
                .GetUserLeaveAllocationByIdAsync(leaveRequesterId, leaveRequestId, currentYear, cancellationToken)
                ?? throw new NotFoundException($"No leave allocation found");

            var currentlyUsedDays = notification.Payload.LeaveRequestCurrentlyUsedDays;

            allocation.ReturnDays(currentlyUsedDays);

            _logger.LogInformation("Returning {UsedDays} days for leave allocation with ID: {Allocation} started", currentlyUsedDays, allocation.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Returning {UsedDays} days for leave allocation with ID: {Allocation} successfull", currentlyUsedDays, allocation.Id);
        }

        _logger.LogInformation("Sending leave request-cancelation email to user with email: {UserEmail}", requestingUserEmail);
        await _emailService.SendLeaveRequestCancelationEmail(
            requestingUserEmail,
            requestingUserName,
            leaveTypeName,
            leaveRequestCreatedAt,
            cancellationToken
        );
    }
}
