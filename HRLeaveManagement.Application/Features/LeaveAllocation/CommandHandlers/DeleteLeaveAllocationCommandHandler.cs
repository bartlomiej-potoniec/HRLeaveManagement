using AutoMapper;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands;
using HRLeaveManagement.Application.Validation;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.CommandHandlers;

public sealed class DeleteLeaveAllocationCommandHandler(ILeaveAllocationRepository repository,
                                                        IAppLogger<DeleteLeaveAllocationCommand> logger,
                                                        IMapper mapper) 
    : IRequestHandler<DeleteLeaveAllocationCommand>
{
    private readonly ILeaveAllocationRepository _repository = repository;
    private readonly IAppLogger<DeleteLeaveAllocationCommand> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task Handle(DeleteLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteLeaveAllocationCommandValidator(_repository);
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("");
            throw new BadRequestException("Invalid leave allocation", validationResult);
        }

        var leaveAllocation = await _repository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(LeaveAllocation), request.Id);

        await _repository.DeleteAsync(leaveAllocation);
    }
}
