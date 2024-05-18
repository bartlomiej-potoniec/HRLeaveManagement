using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.Commands;

public sealed record UpdateLeaveTypeCommand(int Id, string Name, int DefaultDays) : IRequest;
