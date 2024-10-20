using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.Commands;

public sealed record CreateLeaveTypeCommand(string Name, int DefaultDays) : IRequest<int>;
