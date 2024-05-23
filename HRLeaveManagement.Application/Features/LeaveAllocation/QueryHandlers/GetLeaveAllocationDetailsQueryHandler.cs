using AutoMapper;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveAllocation.Queries;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.QueryHandlers;

public sealed class GetLeaveAllocationDetailsQueryHandler(ILeaveAllocationRepository repository,
                                                          IMapper mapper)
    : IRequestHandler<GetLeaveAllocationDetailsQuery, LeaveAllocationDetailsDTO>
{
    private readonly ILeaveAllocationRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<LeaveAllocationDetailsDTO> Handle(GetLeaveAllocationDetailsQuery request,
                                                        CancellationToken cancellationToken)
    {
        var leaveAllocation = await _repository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"No leave allocation with id: {request.Id} found");

        var leaveAllocationDto = _mapper.Map<LeaveAllocationDetailsDTO>(leaveAllocation);
        return leaveAllocationDto;
    }
}
