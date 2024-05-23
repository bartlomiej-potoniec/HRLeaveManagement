using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Commands;

public sealed record CreateLeaveAllocationCommand(int LeaveTypeId) : IRequest<int>;
