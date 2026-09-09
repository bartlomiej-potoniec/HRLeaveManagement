using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.CreateLeaveAllocation.Commands;

public sealed record CreateLeaveAllocationsCommand(int LeaveTypeId, string? LeaveRuleSet) : IRequest;
