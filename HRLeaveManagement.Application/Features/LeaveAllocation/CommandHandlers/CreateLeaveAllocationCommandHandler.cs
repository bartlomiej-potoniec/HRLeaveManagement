using DomainLeaveAllocation = HRLeaveManagement.Domain.LeaveAllocation;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands;
using HRLeaveManagement.Application.Validation;
using MediatR;
using AutoMapper;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.CommandHandlers;

public sealed class CreateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository,
                                                        ILeaveTypeRepository leaveTypeRepository,
                                                        IAppLogger<CreateLeaveAllocationCommand> logger,
                                                        IMapper mapper) 
    : IRequestHandler<CreateLeaveAllocationCommand, int>
{
    private readonly ILeaveAllocationRepository _leaveAllocationRepository = leaveAllocationRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository = leaveTypeRepository;
    private readonly IAppLogger<CreateLeaveAllocationCommand> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<int> Handle(CreateLeaveAllocationCommand request,
                                  CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveAllocationCommandValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("");
            throw new BadRequestException("Invalid leave allocation request", validationResult);
        }

        var leaveType = await _leaveTypeRepository.GetByIdAsync(request.LeaveTypeId);

        // Get Employees

        // Get Period

        var leaveAllocation = _mapper.Map<DomainLeaveAllocation>(request);
        var leaveAllocationId = await _leaveAllocationRepository.CreateAsync(leaveAllocation);

        return leaveAllocationId;
    }
}
