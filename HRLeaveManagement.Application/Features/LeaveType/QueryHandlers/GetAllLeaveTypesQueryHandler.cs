using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.Features.LeaveType.Queries;
using MediatR;
using AutoMapper;

namespace HRLeaveManagement.Application.Features.LeaveType.QueryHandlers;

public sealed class GetAllLeaveTypesQueryHandler(ILeaveTypeRepository repository, IMapper mapper) 
    : IRequestHandler<GetAllLeaveTypesQuery, IEnumerable<LeaveTypeDTO>>
{
    private readonly ILeaveTypeRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<LeaveTypeDTO>> Handle(GetAllLeaveTypesQuery request,
                                                        CancellationToken cancellationToken)
    {
        var leaveTypes = await _repository.GetAllAsync()
            ?? throw new Exception();

        var leaveTypeDtos = _mapper.Map<IEnumerable<LeaveTypeDTO>>(leaveTypes);

        return leaveTypeDtos;
    }
}
