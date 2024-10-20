using DomainLeaveRequest = HRLeaveManagement.Domain.LeaveRequest;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Exceptions;
using MediatR;
using AutoMapper;
using HRLeaveManagement.Application.Contracts.Infrastructure.Email;
using HRLeaveManagement.Application.Models.Email;

namespace HRLeaveManagement.Application.Features.LeaveRequest.CommandHandlers;

public sealed class UpdateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,
                                                     ILeaveTypeRepository leaveTypeRepository,
                                                     IAppLogger<UpdateLeaveRequestCommand> logger,
                                                     IEmailSender emailSender,
                                                     IMapper mapper) 
    : IRequestHandler<UpdateLeaveRequestCommand>
{
    private readonly ILeaveRequestRepository _leaveRequestRepository = leaveRequestRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository = leaveTypeRepository;
    private readonly IAppLogger<UpdateLeaveRequestCommand> _logger = logger;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IMapper _mapper = mapper;

    public async Task Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(LeaveRequest), request.Id);

        var validator = new UpdateLeaveRequestCommandValidator(
            _leaveRequestRepository,
            _leaveTypeRepository
        );

        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("");
            throw new BadRequestException("Leave request is invalid", validationResult);
        }

        _mapper.Map(request, leaveRequest);

        await _leaveRequestRepository.UpdateAsync(leaveRequest);

        try
        {
            var email = new EmailMessage
            {
                To = string.Empty, /* Get email from employee record */
                TextContent = $"Your leave request for {request.StartedAt:D} to {request.EndedAt:D}" +
                          $"has been updated successfully.",
                Subject = $"Leave request with ID: {request.Id} updated"
            };

            await _emailSender.SendEmailAsync(email);
        }
        
        catch (Exception ex)
        {
            _logger.LogWarning(ex.Message);
        }
    }
}
