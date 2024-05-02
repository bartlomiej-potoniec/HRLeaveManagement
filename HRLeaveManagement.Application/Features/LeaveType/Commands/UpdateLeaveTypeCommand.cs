using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.Commands;

public sealed record UpdateLeaveTypeCommand(string Name, int DefaultDays) : IRequest;
