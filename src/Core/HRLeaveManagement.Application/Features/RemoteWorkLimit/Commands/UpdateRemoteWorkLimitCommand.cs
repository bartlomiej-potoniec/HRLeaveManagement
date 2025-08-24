using MediatR;

namespace HRLeaveManagement.Application.Features.RemoteWorkLimit.Commands;

public sealed record UpdateRemoteWorkLimitCommand(int Id,
                                                  int Year,
                                                  int AvailableDays)
    : IRequest;
