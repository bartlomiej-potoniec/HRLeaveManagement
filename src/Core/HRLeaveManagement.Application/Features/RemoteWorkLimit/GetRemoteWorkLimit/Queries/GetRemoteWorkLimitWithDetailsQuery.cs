using HRLeaveManagement.Application.DTOs.RemoteWorkLimits;
using MediatR;

namespace HRLeaveManagement.Application.Features.RemoteWorkLimit.GetRemoteWorkLimit.Queries;

public sealed record GetRemoteWorkLimitWithDetailsQuery(int Id) : IRequest<RemoteWorkLimitDetailsDTO>;
