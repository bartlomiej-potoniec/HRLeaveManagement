using AutoMapper;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.DTOs.Departments;
using HRLeaveManagement.Application.DTOs.RemoteWorkLimits;
using HRLeaveManagement.Application.Features.RemoteWorkLimits.Queries;
using MediatR;

namespace HRLeaveManagement.Application.Features.RemoteWorkLimits.QueryHandlers;

public sealed class GetAllRemoteWorkLimitsQueryHandler(IRemoteWorkLimitRepository remoteWorkLimitRepository,
                                                       IMapper mapper,
                                                       IAppLogger<GetAllRemoteWorkLimitsQueryHandler> logger)
    : IRequestHandler<GetAllRemoteWorkLimitsQuery, IEnumerable<RemoteWorkLimitDTO>>
{
    private readonly IRemoteWorkLimitRepository _remoteWorkLimitRepository = remoteWorkLimitRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetAllRemoteWorkLimitsQueryHandler> _logger = logger;

    public async Task<IEnumerable<RemoteWorkLimitDTO>> Handle(GetAllRemoteWorkLimitsQuery request,
                                                        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all remote work limits started");

        var remoteWorkLimits = await _remoteWorkLimitRepository.GetAllAsync();
        var remoteWorkLimitDtos = _mapper.Map<IEnumerable<RemoteWorkLimitDTO>>(remoteWorkLimits);

        _logger.LogInformation("Fetching all remote work limits successful");

        return remoteWorkLimitDtos;
    }
}
