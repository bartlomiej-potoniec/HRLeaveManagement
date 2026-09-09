using MediatR;

namespace HRLeaveManagement.Application.Features.RemoteWorkLimit.CreateRemoteWorkLimit.Commands;

public sealed record CreateRemoteWorkLimitCommand(Guid EmployeeId,
                                                  int Year,
                                                  int AvailableDays)
    : IRequest<int>;
