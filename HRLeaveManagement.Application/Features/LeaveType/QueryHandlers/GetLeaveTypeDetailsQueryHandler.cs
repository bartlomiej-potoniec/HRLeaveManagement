using AutoMapper;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveType.Queries;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.QueryHandlers;

public sealed class GetLeaveTypeDetailsQueryHandler(ILeaveTypeRepository repository, IMapper mapper)
    : IRequestHandler<GetLeaveTypeDetailsQuery, LeaveTypeDetailsDTO>
{
    private readonly ILeaveTypeRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<LeaveTypeDetailsDTO> Handle(GetLeaveTypeDetailsQuery request,
                                                  CancellationToken cancellationToken)
    {
        var leaveType = await _repository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(LeaveType), request.Id);
        
        var leaveTypeDetailsDto = _mapper.Map<LeaveTypeDetailsDTO>(leaveType);
        return leaveTypeDetailsDto;
    }
}
