using DomainLeaveRequest = HRLeaveManagement.Domain.Entities.LeaveRequest;
using HRLeaveManagement.Application.Contracts.Infrastructure.Email;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands;
using HRLeaveManagement.Application.DTOs.Email;
using HRLeaveManagement.Application.Exceptions;
using MediatR;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;

namespace HRLeaveManagement.Application.Features.LeaveRequest.CommandHandlers;

public sealed class CancelLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,
                                                     ILeaveAllocationRepository leaveAllocationRepository,
                                                     IEmailSender emailSender,
                                                     IAppLogger<CancelLeaveRequestCommand> logger) 
    : IRequestHandler<CancelLeaveRequestCommand>
{
    public readonly ILeaveRequestRepository _leaveRequestRepository = leaveRequestRepository;
    public readonly ILeaveAllocationRepository _leaveAllocationRepository = leaveAllocationRepository;
    public readonly IEmailSender _emailSender = emailSender;
    private readonly IAppLogger<CancelLeaveRequestCommand> _logger = logger;

    public async Task Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(LeaveRequest), request.Id);

        leaveRequest.IsCanceled = true;
        await _leaveRequestRepository.UpdateAsync(leaveRequest);

        // Reevaluate the employee's allocations for the leave type
        if (leaveRequest.IsApproved is false) return;

        int daysRequested = (int)(leaveRequest.EndedAt - leaveRequest.StartedAt).TotalDays;

        var allocation = await _leaveAllocationRepository
            .GetUserLeaveAllocationsByIdAsync(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId)
            ?? throw new NotFoundException($"No allocations for user with id: { leaveRequest.RequestingEmployeeId } found");

        allocation.NumberOfDays += daysRequested;

        await _leaveAllocationRepository.UpdateAsync(allocation);

        await TrySendEmail(leaveRequest);
    }

    private async Task TrySendEmail(DomainLeaveRequest leaveRequest, CancellationToken cancellationToken)
    {
        try
        {
            var email = new EmailMessage
            {
                To = string.Empty, /* Get email from employee record */
                TextContent = $"Your leave request for {leaveRequest.StartedAt:D} to {leaveRequest.EndedAt:D}" +
                              $"has been submitted successfully.",
                Subject = $"Leave request with ID: {leaveRequest.Id} submitted"
            };

            await _emailSender.SendEmailAsync(email, cancellationToken);
        }

        catch (Exception ex)
        {
            _logger.LogWarning(ex.Message);
        }
    }
}
