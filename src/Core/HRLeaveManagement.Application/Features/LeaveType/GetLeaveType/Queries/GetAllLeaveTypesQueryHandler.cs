using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.DTOs.LeaveTypes;
using MediatR;
using AutoMapper;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;

namespace HRLeaveManagement.Application.Features.LeaveType.GetLeaveType.Queries;

public sealed class GetAllLeaveTypesQueryHandler(ILeaveTypeRepository leaveTypeRepository,
                                                 IMapper mapper,
                                                 IAppLogger<GetAllLeaveTypesQueryHandler> logger) 
    : IRequestHandler<GetAllLeaveTypesQuery, IEnumerable<LeaveTypeDTO>>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository = leaveTypeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetAllLeaveTypesQueryHandler> _logger = logger;

    public async Task<IEnumerable<LeaveTypeDTO>> Handle(GetAllLeaveTypesQuery request,
                                                        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all leave types started");

        var leaveTypes = await _leaveTypeRepository.GetAllAsync(cancellationToken);
        var leaveTypeDtos = _mapper.Map<IEnumerable<LeaveTypeDTO>>(leaveTypes);

        _logger.LogInformation("Fetching all leave types successful");

        return leaveTypeDtos;
    }
}
