using DomainLeaveRequest = HRLeaveManagement.Domain.Entities.LeaveRequest;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands;
using HRLeaveManagement.Application.Contracts.Infrastructure.Email;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.DTOs.Email;
using MediatR;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;

namespace HRLeaveManagement.Application.Features.LeaveRequest.CommandHandlers;

public sealed class ChangeLeaveRequestApprovalCommandHandler(ILeaveRequestRepository leaveRequestRepository,
                                                             ILeaveAllocationRepository leaveAllocationRepository,
                                                             IEmailSender emailSender,
                                                             IAppLogger<ChangeLeaveRequestApprovalCommand> logger)
    : IRequestHandler<ChangeLeaveRequestApprovalCommand>
{
    private readonly ILeaveRequestRepository _leaveRequestRepository = leaveRequestRepository;
    private readonly ILeaveAllocationRepository _leaveAllocationRepository = leaveAllocationRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IAppLogger<ChangeLeaveRequestApprovalCommand> _logger = logger;

    public async Task Handle(ChangeLeaveRequestApprovalCommand request, CancellationToken cancellationToken)
    {
        /*var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(LeaveRequest), request.Id);

        leaveRequest.IsApproved = request.IsApproved;
        await _leaveRequestRepository.UpdateAsync(leaveRequest);

        // if request is approved get and update the employee's allocations
        if (request.IsApproved)
            await UpdateEmployeeAllocations(leaveRequest);

        await TrySendEmail(request, leaveRequest);*/
    }

    private async Task UpdateEmployeeAllocations(DomainLeaveRequest leaveRequest, CancellationToken cancellationToken)
    {
        /*var requestedDays = (int)(leaveRequest.EndedAt - leaveRequest.StartedAt).TotalDays;

        var allocation = await _leaveAllocationRepository
            .GetUserLeaveAllocationsByIdAsync(
                leaveRequest.RequestingEmployeeId,
                leaveRequest.LeaveTypeId
            )
            ?? throw new NotFoundException(
                $"No allocations for user with id: { leaveRequest.RequestingEmployeeId }"
            );

        allocation.NumberOfDays -= requestedDays;

        await _leaveAllocationRepository.UpdateAsync(allocation);*/
    }

    private async Task TrySendEmail(ChangeLeaveRequestApprovalCommand request,
                                    DomainLeaveRequest leaveRequest, 
                                    CancellationToken cancellationToken)
    {
        try
        {
            var email = new EmailMessage
            {
                To = string.Empty, /* Get email from employee record */
                TextContent = $"Status of your leave request for {leaveRequest.StartedAt:D} to {leaveRequest.EndedAt:D}" +
                          $"has been changed on { (request.IsApproved ? "approved" : "rejected") }",
                Subject = $"Status of leave request with ID: { leaveRequest.Id } changed"
            };

            await _emailSender.SendEmailAsync(email, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex.Message);
        }
    }
}
