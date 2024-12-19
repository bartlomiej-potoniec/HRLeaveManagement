using DomainLeaveAllocation = HRLeaveManagement.Domain.Entities.LeaveAllocation;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Exceptions;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.CommandHandlers;

public sealed class UpdateLeaveAllocationCommandHandler(ILeaveTypeRepository leaveTypeRepository,
                                                        ILeaveAllocationRepository leaveAllocationRepository,
                                                        IAppLogger<UpdateLeaveAllocationCommand> logger) 
    : IRequestHandler<UpdateLeaveAllocationCommand>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository = leaveTypeRepository;
    private readonly ILeaveAllocationRepository _leaveAllocationRepository = leaveAllocationRepository;
    private readonly IAppLogger<UpdateLeaveAllocationCommand> _logger = logger;

    public async Task Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateLeaveAllocationCommandValidator(_leaveAllocationRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(UpdateLeaveAllocationCommand));
            throw new BadRequestException("Invalid leave allocation updating request", validationResult);
        }

        var leaveAllocation = await _leaveAllocationRepository
            .GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"No leave allocation with ID: { request.Id } found");

        DomainLeaveAllocation.Update(leaveAllocation, request.AvailableDays);

        _logger.LogInformation("Updating informations about leave allocation with ID: {Id} started", request.Id);

        await _leaveAllocationRepository.UpdateAsync(leaveAllocation);

        _logger.LogInformation("Updating informations about leave allocation with ID: {Id} successful", request.Id);
    }
}
