using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.DTOs.LeaveAllocations;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveAllocation.Queries;
using MediatR;
using AutoMapper;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.QueryHandlers;

public sealed class GetLeaveAllocationWithDetailsQueryHandler(ILeaveAllocationRepository leaveAllocationRepository,
                                                              IMapper mapper,
                                                              IAppLogger<GetLeaveAllocationWithDetailsQueryHandler> logger)
    : IRequestHandler<GetLeaveAllocationWithDetailsQuery, LeaveAllocationDetailsDTO>
{
    private readonly ILeaveAllocationRepository _leaveAllocationRepository = leaveAllocationRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetLeaveAllocationWithDetailsQueryHandler> _logger = logger;

    public async Task<LeaveAllocationDetailsDTO> Handle(GetLeaveAllocationWithDetailsQuery request,
                                                        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching leave allocation with ID: {Id} started", request.Id);

        var leaveAllocation = await _leaveAllocationRepository
            .GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"No leave allocation with id: {request.Id} found");

        var leaveAllocationDto = _mapper.Map<LeaveAllocationDetailsDTO>(leaveAllocation);

        _logger.LogInformation("Fetching leave allocation with ID: {Id} successful", request.Id);

        return leaveAllocationDto;
    }
}
