using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.Features.LeaveType.Queries;
using MediatR;
using AutoMapper;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;

namespace HRLeaveManagement.Application.Features.LeaveType.QueryHandlers;

public sealed class GetAllLeaveTypesQueryHandler(ILeaveTypeRepository repository,
                                                 IMapper mapper,
                                                 IAppLogger<GetAllLeaveTypesQueryHandler> logger) 
    : IRequestHandler<GetAllLeaveTypesQuery, IEnumerable<LeaveTypeDTO>>
{
    private readonly ILeaveTypeRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetAllLeaveTypesQueryHandler> _logger = logger;

    public async Task<IEnumerable<LeaveTypeDTO>> Handle(GetAllLeaveTypesQuery request,
                                                        CancellationToken cancellationToken)
    {
        var leaveTypes = await _repository.GetAllAsync()
            ?? throw new Exception();

        var leaveTypeDtos = _mapper.Map<IEnumerable<LeaveTypeDTO>>(leaveTypes);

        _logger.LogInformation("Leave types were retrieved successfully");

        return leaveTypeDtos;
    }
}
