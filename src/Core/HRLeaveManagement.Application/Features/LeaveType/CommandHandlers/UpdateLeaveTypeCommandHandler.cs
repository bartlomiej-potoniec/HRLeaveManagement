using HRLeaveManagement.Domain.RuleContracts;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Features.LeaveType.Commands;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Exceptions;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.CommandHandlers;

public sealed class UpdateLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository,
                                                    ILeaveTypeRuleSet leaveTypeRuleSet,
                                                  IAppLogger<UpdateLeaveTypeCommandHandler> logger)
    : IRequestHandler<UpdateLeaveTypeCommand>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository = leaveTypeRepository;
    private readonly ILeaveTypeRuleSet _leaveTypeRuleSet = leaveTypeRuleSet;
    private readonly IAppLogger<UpdateLeaveTypeCommandHandler> _logger = logger;

    public async Task Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateLeaveTypeCommandValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(UpdateLeaveTypeCommand));
            throw new BadRequestException("Invalid leave type updating request", validationResult);
        }

        var leaveType = await _leaveTypeRepository
            .GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"No leave type with ID: { request.Id } found");

        await leaveType.UpdateAsync(
            _leaveTypeRuleSet,
            request.Name,
            request.PaidFraction,
            request.Description,
            cancellationToken
        );
        
        _logger.LogInformation("Updating informations about leave type with ID: {Id} started", request.Id);

        await _leaveTypeRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updating informations about leave type with ID: {Id} successful", request.Id);
    }
}
