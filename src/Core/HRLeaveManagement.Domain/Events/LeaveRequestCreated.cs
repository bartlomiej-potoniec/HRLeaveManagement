using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.Events;

public sealed class LeaveRequestCreated(LeaveRequest leaveRequest) : IEntityEvent
{
    public LeaveRequest LeaveRequest => leaveRequest;

    public string LeaveTypeName => leaveRequest.LeaveType.Name;
    public DateOnly LeaveStartedAt => leaveRequest.StartedAt;
    public DateOnly LeaveEndedAt => leaveRequest.EndedAt;

    public string RequesterFullName 
        => $"{ leaveRequest.RequestingEmployee.FirstName } { leaveRequest.RequestingEmployee.LastName }";
    public string ApproverFullName
        => $"{ leaveRequest.Approver.FirstName } { leaveRequest.Approver.LastName }";

    public DateTime OccurredOn => DateTime.UtcNow;

    public string Content => $"Wystawiono wniosek o { LeaveTypeName } na { LeaveStartedAt }-{ LeaveEndedAt }";
}
