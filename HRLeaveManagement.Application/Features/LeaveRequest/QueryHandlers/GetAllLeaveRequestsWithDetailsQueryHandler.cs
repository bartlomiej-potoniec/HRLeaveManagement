using AutoMapper;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.Features.LeaveRequest.Queries;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.QueryHandlers;

public sealed class GetAllLeaveRequestsWithDetailsQueryHandler(ILeaveRequestRepository repository,
                                                               IMapper mapper)
    : IRequestHandler<GetAllLeaveRequestsWithDetailsQuery, IEnumerable<LeaveRequestDTO>>
{
    private readonly ILeaveRequestRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<LeaveRequestDTO>> Handle(GetAllLeaveRequestsWithDetailsQuery request,
                                                           CancellationToken cancellationToken)
    {
        // TODO: Check if it is logged in employee

        var leaveRequests = await _repository.GetAllLeaveRequestsWithDetailsAsync();
        var leaveRequestDtos = _mapper.Map<IEnumerable<LeaveRequestDTO>>(leaveRequests);

        // TODO: Fill requests with employee information

        return leaveRequestDtos;
    }
}
