using DomainLeaveRequest = HRLeaveManagement.Domain.LeaveRequest;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Models.Email;
using HRLeaveManagement.Application.Contracts.Infrastructure.Email;
using HRLeaveManagement.Application.Contracts.Identity;
using MediatR;
using AutoMapper;
using FluentValidation.Results;

namespace HRLeaveManagement.Application.Features.LeaveRequest.CommandHandlers;

public sealed class CreateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,
                                                     ILeaveTypeRepository leaveTypeRepository,
                                                     ILeaveAllocationRepository leaveAllocationRepository,
                                                     IUserService userService,
                                                     IEmailSender emailSender,
                                                     IAppLogger<CreateLeaveRequestCommand> logger,
                                                     IMapper mapper)
    : IRequestHandler<CreateLeaveRequestCommand, int>
{
    private readonly ILeaveRequestRepository _leaveRequestRepository = leaveRequestRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository = leaveTypeRepository;
    private readonly ILeaveAllocationRepository _leaveAllocationRepository = leaveAllocationRepository;
    private readonly IUserService _userService = userService;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IAppLogger<CreateLeaveRequestCommand> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<int> Handle(CreateLeaveRequestCommand request,
                                  CancellationToken cancellationToken)
    {
        var employeeId = _userService.UserId
            ?? throw new NotFoundException("No user claim exists in actual context");

        var validator = new CreateLeaveRequestCommandValidator(
            _leaveTypeRepository,
            _leaveAllocationRepository,
            employeeId
        );

        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("");
            throw new BadRequestException("Invalid leave request", validationResult);
        }

        var leaveRequest = _mapper.Map<DomainLeaveRequest>(request, opt =>
            opt.AfterMap((src, dest) => dest.RequestingEmployeeId = employeeId)
        );

        var leaveRequestId = await _leaveRequestRepository.CreateAsync(leaveRequest);

        try
        {
            var email = new EmailMessage
            {
                To = string.Empty, /* Get email from employee record */
                TextContent = $"Your leave request for {request.StartedAt:D} to {request.EndedAt:D}" +
                              $"has been submitted successfully.",
                Subject = $"Leave request with ID: {leaveRequestId} submitted"
            };

            await _emailSender.SendEmailAsync(email);
        }

        catch (Exception ex)
        {
            _logger.LogWarning(ex.Message);
        }

        return leaveRequestId;
    }
}
