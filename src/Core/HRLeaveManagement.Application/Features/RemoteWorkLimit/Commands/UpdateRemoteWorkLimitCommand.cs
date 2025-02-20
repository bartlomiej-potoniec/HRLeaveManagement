using MediatR;

namespace HRLeaveManagement.Application.Features.RemoteWorkLimit.Commands;

public sealed record UpdateRemoteWorkLimitCommand(int Id,
                                                  Guid EmployeeId,
                                                  int Year,
                                                  int AvailableDays)
    : IRequest;
