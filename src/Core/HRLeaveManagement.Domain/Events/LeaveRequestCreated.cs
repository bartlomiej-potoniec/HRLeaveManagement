using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.Events;

public sealed class LeaveRequestCreated(LeaveRequest leaveRequest) : IEntityEvent
{
    public LeaveRequest LeaveRequest => leaveRequest;
    public DateTime OccurredOn => DateTime.UtcNow;

    public string Content => "Created Leave Request";
}
