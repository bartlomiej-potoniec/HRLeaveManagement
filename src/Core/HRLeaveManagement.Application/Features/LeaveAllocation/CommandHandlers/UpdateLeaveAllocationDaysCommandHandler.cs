using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Exceptions;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.CommandHandlers;

public sealed class UpdateLeaveAllocationDaysCommandHandler(ILeaveTypeRepository leaveTypeRepository,
                                                            ILeaveAllocationRepository leaveAllocationRepository,
                                                            IAppLogger<UpdateLeaveAllocationDaysCommand> logger) 
    : IRequestHandler<UpdateLeaveAllocationDaysCommand>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository = leaveTypeRepository;
    private readonly ILeaveAllocationRepository _leaveAllocationRepository = leaveAllocationRepository;
    private readonly IAppLogger<UpdateLeaveAllocationDaysCommand> _logger = logger;

    public async Task Handle(UpdateLeaveAllocationDaysCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateLeaveAllocationCommandValidator(_leaveAllocationRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(UpdateLeaveAllocationDaysCommand));
            throw new BadRequestException("Invalid leave allocation updating request", validationResult);
        }

        var leaveAllocation = await _leaveAllocationRepository
            .GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"No leave allocation with ID: { request.Id } found");

        leaveAllocation.UpdateDays(request.AvailableDays);

        _logger.LogInformation("Updating informations about leave allocation with ID: {Id} started", request.Id);

        await _leaveAllocationRepository.UpdateAsync(leaveAllocation, cancellationToken);

        _logger.LogInformation("Updating informations about leave allocation with ID: {Id} successful", request.Id);
    }
}
