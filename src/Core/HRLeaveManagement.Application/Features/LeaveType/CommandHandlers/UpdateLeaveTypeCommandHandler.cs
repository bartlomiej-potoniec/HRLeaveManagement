using DomainLeaveType = HRLeaveManagement.Domain.Entities.LeaveType;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveType.Commands;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Exceptions;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.CommandHandlers;

public sealed class UpdateLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository,
                                                  IAppLogger<UpdateLeaveTypeCommandHandler> logger)
    : IRequestHandler<UpdateLeaveTypeCommand>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository = leaveTypeRepository;
    private readonly IAppLogger<UpdateLeaveTypeCommandHandler> _logger = logger;

    public async Task Handle(UpdateLeaveTypeCommand request,
                             CancellationToken cancellationToken)
    {
        var validator = new UpdateLeaveTypeCommandValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(UpdateLeaveTypeCommand));
            throw new BadRequestException("Invalid leave type updating request", validationResult);
        }

        var leaveType = await _leaveTypeRepository
            .GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"No leave type with ID: { request.Id } found");

        DomainLeaveType.Update(
            leaveType,
            request.Name,
            request.Description,
            request.PaidFraction
        );
        
        _logger.LogInformation("Updating informations about leave type with ID: {Id} started", request.Id);

        await _leaveTypeRepository.UpdateAsync(leaveType);

        _logger.LogInformation("Updating informations about leave type with ID: {Id} successful", request.Id);
    }
}
