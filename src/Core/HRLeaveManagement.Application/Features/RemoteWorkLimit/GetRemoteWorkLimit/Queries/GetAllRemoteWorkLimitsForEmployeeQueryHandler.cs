using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.DTOs.RemoteWorkLimits;
using MediatR;
using AutoMapper;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;

namespace HRLeaveManagement.Application.Features.RemoteWorkLimit.GetRemoteWorkLimit.Queries;

public sealed class GetAllRemoteWorkLimitsForEmployeeQueryHandler(IRemoteWorkLimitRepository remoteWorkLimitRepository,
                                                                  IMapper mapper,
                                                                  IAppLogger<GetAllRemoteWorkLimitsForEmployeeQueryHandler> logger)
    : IRequestHandler<GetAllRemoteWorkLimitsForEmployeeQuery, IEnumerable<RemoteWorkLimitDTO>>
{
    private readonly IRemoteWorkLimitRepository _remoteWorkLimitRepository = remoteWorkLimitRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetAllRemoteWorkLimitsForEmployeeQueryHandler> _logger = logger;

    public async Task<IEnumerable<RemoteWorkLimitDTO>> Handle(GetAllRemoteWorkLimitsForEmployeeQuery request,
                                                              CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all remote work limits for employee with ID: {EmployeeId} started", request.EmployeeId);

        var remoteWorkLimits = await _remoteWorkLimitRepository
            .GetAllRemoteWorkLimitsByEmployeeIdAsync(request.EmployeeId, cancellationToken);

        var remoteWorkLimitDtos = _mapper.Map<IEnumerable<RemoteWorkLimitDTO>>(remoteWorkLimits);

        _logger.LogInformation("Fetching all remote work limits for employee with ID: {EmployeeId} successful", request.EmployeeId);

        return remoteWorkLimitDtos;
    }
}
