using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Exceptions;
using MediatR;
using AutoMapper;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.CommandHandlers;

public sealed class UpdateLeaveAllocationCommandHandler(ILeaveTypeRepository leaveTypeRepository,
                                                        ILeaveAllocationRepository leaveAllocationRepository,
                                                        IAppLogger<UpdateLeaveAllocationCommand> logger,
                                                        IMapper mapper) 
    : IRequestHandler<UpdateLeaveAllocationCommand>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository = leaveTypeRepository;
    private readonly ILeaveAllocationRepository _leaveAllocationRepository = leaveAllocationRepository;
    private readonly IAppLogger<UpdateLeaveAllocationCommand> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateLeaveAllocationCommandValidator(
            _leaveTypeRepository, 
            _leaveAllocationRepository
        );

        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("");
            throw new BadRequestException("Invalid leave allocation", validationResult);
        }

        var leaveAllocation = await _leaveAllocationRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(LeaveAllocation), request.Id);

        _mapper.Map(request, leaveAllocation);

        await _leaveAllocationRepository.UpdateAsync(leaveAllocation);
    }
}
