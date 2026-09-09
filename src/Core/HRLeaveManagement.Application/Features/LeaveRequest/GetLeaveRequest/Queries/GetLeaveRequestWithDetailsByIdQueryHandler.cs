using AutoMapper;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.Exceptions;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.GetLeaveRequest.Queries;

public sealed class GetLeaveRequestWithDetailsByIdQueryHandler(ILeaveRequestRepository repository,
                                                               IMapper mapper)
    : IRequestHandler<GetLeaveRequestWithDetailsByIdQuery, LeaveRequestDetailsDTO>
{
    private readonly ILeaveRequestRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<LeaveRequestDetailsDTO> Handle(GetLeaveRequestWithDetailsByIdQuery request,
                                              CancellationToken cancellationToken)
    {
        var leaveRequest = await _repository.GetLeaveRequestWithDetailsByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(LeaveRequest), request.Id);

        var leaveRequestDto = _mapper.Map<LeaveRequestDetailsDTO>(leaveRequest);

        // TODO: Add employee details as needed
        
        return leaveRequestDto;
    }
}
