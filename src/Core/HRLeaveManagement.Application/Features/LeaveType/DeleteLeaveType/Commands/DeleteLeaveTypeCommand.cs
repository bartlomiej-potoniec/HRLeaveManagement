using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.DeleteLeaveType.Commands;

public sealed record DeleteLeaveTypeCommand(int Id) : IRequest;
