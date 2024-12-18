using HRLeaveManagement.Application.DTOs.RemoteWorkLimits;
using MediatR;

namespace HRLeaveManagement.Application.Features.RemoteWorkLimits.Queries;

public sealed record GetAllRemoteWorkLimitsQuery : IRequest<IEnumerable<RemoteWorkLimitDTO>>;
