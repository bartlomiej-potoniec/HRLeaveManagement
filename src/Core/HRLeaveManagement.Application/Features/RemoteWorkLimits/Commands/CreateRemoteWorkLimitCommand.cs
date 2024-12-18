using MediatR;

namespace HRLeaveManagement.Application.Features.RemoteWorkLimits.Commands;

public sealed record CreateRemoteWorkLimitCommand(Guid EmployeeId,
                                                  int Year,
                                                  int AvailableDays)
    : IRequest<int>;
