using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveType.Commands;
using HRLeaveManagement.Application.Validation;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.CommandHandlers;

public sealed class DeleteLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository,
                                                  IAppLogger<DeleteLeaveTypeCommand> logger) 
    : IRequestHandler<DeleteLeaveTypeCommand>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository = leaveTypeRepository;
    private readonly IAppLogger<DeleteLeaveTypeCommand> _logger = logger;

    public async Task Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteLeaveTypeCommandValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(DeleteLeaveTypeCommand));
            throw new BadRequestException("Invalid leave type deleting request", validationResult);
        }

        var leaveType = await _leaveTypeRepository
            .GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"No Leave type with ID: { request.Id } found");

        _logger.LogInformation("Deleting leave type with ID: {Id} started", request.Id);

        await _leaveTypeRepository.DeleteAsync(leaveType, cancellationToken);

        _logger.LogInformation("Deleting leave type with ID: {Id} successful", request.Id);
    }
}
