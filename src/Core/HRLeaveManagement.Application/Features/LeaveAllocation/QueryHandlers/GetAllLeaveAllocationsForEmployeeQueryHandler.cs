using AutoMapper;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.DTOs.LeaveAllocations;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveAllocation.Queries;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.QueryHandlers;

public sealed class GetAllLeaveAllocationsForEmployeeQueryHandler(ILeaveAllocationRepository leaveAllocationRepository,
                                                                  IMapper mapper,
                                                                  IAppLogger<GetAllLeaveAllocationsForEmployeeQueryHandler> logger)
    : IRequestHandler<GetAllLeaveAllocationsForEmployeeQuery, IEnumerable<LeaveAllocationDetailsDTO>>
{
    private readonly ILeaveAllocationRepository _leaveAllocationRepository = leaveAllocationRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetAllLeaveAllocationsForEmployeeQueryHandler> _logger = logger;

    public async Task<IEnumerable<LeaveAllocationDetailsDTO>> Handle(GetAllLeaveAllocationsForEmployeeQuery request,
                                                                     CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching leave allocations for employee with ID: {Id} started", request.EmployeeId);

        var leaveAllocations = await _leaveAllocationRepository
            .GetAllByEmployeeIdAsync(request.EmployeeId)
            ?? throw new NotFoundException($"No leave allocations for employee with ID: {request.EmployeeId} found");

        var leaveAllocationDtos = _mapper.Map<IEnumerable<LeaveAllocationDetailsDTO>>(leaveAllocations);

        _logger.LogInformation("Fetching leave allocations for employee with ID: {Id} successful", request.EmployeeId);

        return leaveAllocationDtos;
    }
}
