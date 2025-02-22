using DomainLeaveType = HRLeaveManagement.Domain.Entities.LeaveType;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveType.Commands;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.CommandHandlers;

public sealed class CreateLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository,
                                                  IAppLogger<CreateLeaveTypeCommandHandler> logger)
    : IRequestHandler<CreateLeaveTypeCommand, int>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository = leaveTypeRepository;
    private readonly IAppLogger<CreateLeaveTypeCommandHandler> _logger = logger;
 
    public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveTypeCommandValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation errors in create request for {LeaveType}", nameof(LeaveType));
            throw new BadRequestException("Invalid leave type creating request", validationResult);
        }

        var leaveType = DomainLeaveType.Create(
            request.Name,
            request.Description,
            request.PaidFraction
        );

        _logger.LogInformation("Creating new leave type '{Name}' started", request.Name);

        await _leaveTypeRepository.CreateAsync(leaveType, cancellationToken);

        _logger.LogInformation("Creating new leave type '{Name}' successsful", request.Name);

        return leaveType.Id;
    }
}
