using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.DTOs.RemoteWorkLimits;
using HRLeaveManagement.Application.Features.RemoteWorkLimit.Queries;
using MediatR;
using AutoMapper;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;

namespace HRLeaveManagement.Application.Features.RemoteWorkLimit.QueryHandlers;

public sealed class GetRemoteWorkLimitWithDetailsQueryHandler(IRemoteWorkLimitRepository remoteWorkLimitRepository,
                                                              IMapper mapper,
                                                              IAppLogger<GetRemoteWorkLimitWithDetailsQueryHandler> logger)
    : IRequestHandler<GetRemoteWorkLimitWithDetailsQuery, RemoteWorkLimitDetailsDTO>
{
    private readonly IRemoteWorkLimitRepository _remoteWorkLimitRepository = remoteWorkLimitRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetRemoteWorkLimitWithDetailsQueryHandler> _logger = logger;

    public async Task<RemoteWorkLimitDetailsDTO> Handle(GetRemoteWorkLimitWithDetailsQuery request,
                                                        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching department with ID: {Id} started", request.Id);

        var remoteWorkLimit = await _remoteWorkLimitRepository.GetByIdAsync(request.Id, cancellationToken);
        var remoteWorkLimitDto = _mapper.Map<RemoteWorkLimitDetailsDTO>(remoteWorkLimit);

        _logger.LogInformation("Fetching department with ID: {Id} successful", request.Id);

        return remoteWorkLimitDto;
    }
}
