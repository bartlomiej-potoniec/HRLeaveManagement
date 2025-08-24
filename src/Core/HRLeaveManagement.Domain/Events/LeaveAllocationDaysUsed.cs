using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.Events;

public sealed class LeaveAllocationDaysUsed(LeaveAllocation leaveAllocation) : IEntityEvent
{
    public LeaveAllocation LeaveAllocation => leaveAllocation;

    public string Content => $"Days in leave allocation with ID: { LeaveAllocation.Id } used";
    public DateTime OccurredOn => DateTime.UtcNow;
}
