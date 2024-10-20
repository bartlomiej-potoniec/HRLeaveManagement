using AutoMapper;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.Features.LeaveAllocation.Queries;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.QueryHandlers;

public sealed class GetAllLeaveAllocationsQueryHandler(ILeaveAllocationRepository repository,
                                                       IMapper mapper)
    : IRequestHandler<GetAllLeaveAllocationsQuery, IEnumerable<LeaveAllocationDTO>>
{
    private readonly ILeaveAllocationRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<LeaveAllocationDTO>> Handle(GetAllLeaveAllocationsQuery request,
                                                              CancellationToken cancellationToken)
    {
        // TODO: Get records for specific user
        // TODO: Get allocations per employees

        var leaveAllocations = await _repository.GetAllAsync();
        var leaveAllocationDtos = _mapper.Map<IEnumerable<LeaveAllocationDTO>>(leaveAllocations);

        return leaveAllocationDtos;
    }
}
