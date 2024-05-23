using AutoMapper;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveRequest.Queries;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.QueryHandlers;

public sealed class GetLeaveRequestWithDetailsByIdQueryHandler(ILeaveRequestRepository repository,
                                                               IMapper mapper)
    : IRequestHandler<GetLeaveRequestWithDetailsByIdQuery, LeaveRequestDetailsDTO>
{
    private readonly ILeaveRequestRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<LeaveRequestDetailsDTO> Handle(GetLeaveRequestWithDetailsByIdQuery request,
                                              CancellationToken cancellationToken)
    {
        var leaveRequest = await _repository.GetLeaveRequestWithDetailsByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(LeaveRequest), request.Id);

        var leaveRequestDto = _mapper.Map<LeaveRequestDetailsDTO>(leaveRequest);

        // TODO: Add employee details as needed
        
        return leaveRequestDto;
    }
}
