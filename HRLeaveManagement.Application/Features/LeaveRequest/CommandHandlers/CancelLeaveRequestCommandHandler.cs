using HRLeaveManagement.Application.Contracts.Infrastructure.Email;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands;
using HRLeaveManagement.Application.Models.Email;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.CommandHandlers;

public sealed class CancelLeaveRequestCommandHandler(ILeaveRequestRepository repository,
                                                     IEmailSender emailSender,
                                                     IAppLogger<CancelLeaveRequestCommand> logger) 
    : IRequestHandler<CancelLeaveRequestCommand>
{
    public readonly ILeaveRequestRepository _repository = repository;
    public readonly IEmailSender _emailSender = emailSender;
    private readonly IAppLogger<CancelLeaveRequestCommand> _logger = logger;

    public async Task Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _repository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(LeaveRequest), request.Id);

        leaveRequest.IsCanceled = true;

        // Reevaluate the employee's allocations for the leave type

        try
        {
            var email = new EmailMessage
            {
                To = string.Empty, /* Get email from employee record */
                TextContent = $"Your leave request for {leaveRequest.StartedAt:D} to {leaveRequest.EndedAt:D}" +
                              $"has been submitted successfully.",
                Subject = $"Leave request with ID: {leaveRequest.Id} submitted"
            };

            await _emailSender.SendEmailAsync(email);
        }

        catch (Exception ex)
        {
            _logger.LogWarning(ex.Message);
        }
    }
}
