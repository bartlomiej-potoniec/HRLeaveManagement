using HRLeaveManagement.Application.Contracts.Infrastructure.Email;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands;
using HRLeaveManagement.Application.Models.Email;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.CommandHandlers;

public sealed class ChangeLeaveRequestApprovalCommandHandler(ILeaveRequestRepository repository,
                                                             IEmailSender emailSender,
                                                             IAppLogger<ChangeLeaveRequestApprovalCommand> logger)
    : IRequestHandler<ChangeLeaveRequestApprovalCommand>
{
    private readonly ILeaveRequestRepository _repository = repository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IAppLogger<ChangeLeaveRequestApprovalCommand> _logger = logger;

    public async Task Handle(ChangeLeaveRequestApprovalCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _repository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(LeaveRequest), request.Id);

        leaveRequest.IsApproved = request.IsApproved;
        await _repository.UpdateAsync(leaveRequest);

        try
        {
            var email = new EmailMessage
            {
                To = string.Empty, /* Get email from employee record */
                TextContent = $"Status of your leave request for {leaveRequest.StartDate:D} to {leaveRequest.EndDate:D}" +
                          $"has been changed on {(request.IsApproved ? "approved" : "rejected")}",
                Subject = $"Status of leave request with ID: {leaveRequest.Id} changed"
            };

            await _emailSender.SendEmailAsync(email);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex.Message);
        }
    }
}
