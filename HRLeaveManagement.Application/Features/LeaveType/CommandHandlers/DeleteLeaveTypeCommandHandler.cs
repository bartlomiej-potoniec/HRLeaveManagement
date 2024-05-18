using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveType.Commands;
using HRLeaveManagement.Application.Validation;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.CommandHandlers;

public sealed class DeleteLeaveTypeCommandHandler(ILeaveTypeRepository repository,
                                                  IAppLogger<DeleteLeaveTypeCommand> logger) 
    : IRequestHandler<DeleteLeaveTypeCommand>
{
    private readonly ILeaveTypeRepository _repository = repository;
    private readonly IAppLogger<DeleteLeaveTypeCommand> _logger = logger;

    public async Task Handle(DeleteLeaveTypeCommand request,
                             CancellationToken cancellationToken)
    {
        var validator = new DeleteLeaveTypeCommandValidator(_repository);
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation errors in delete request for {0} - {1}", nameof(LeaveType), request.Id);
            throw new BadRequestException("Invalid Leave type", validationResult);
        }

        var leaveType = await _repository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Leave type with id: { request.Id } not found");

        await _repository.DeleteAsync(leaveType);
    }
}
