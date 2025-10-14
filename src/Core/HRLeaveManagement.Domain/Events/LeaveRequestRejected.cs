using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.Events;

public sealed class LeaveRequestRejected(LeaveRequest leaveRequest) : IEntityEvent
{
    public LeaveRequest LeaveRequest => leaveRequest;
    public int LeaveRequestId => leaveRequest.Id;
    public Guid LeaveRequesterId => leaveRequest.RequestingEmployeeId;
    public string LeaveTypeName => leaveRequest.LeaveType.Name;
    public DateTime LeaveRequestCreatedAt => leaveRequest.CreatedAt;

    public string RequesterFullName =>
        $"{leaveRequest.RequestingEmployee.FirstName} {leaveRequest.RequestingEmployee.LastName}";

    public string ApproverFullName =>
        $"{leaveRequest.Approver.FirstName} {leaveRequest.Approver.LastName}";

    public DateTime OccurredOn => DateTime.UtcNow;
    public string Content => $"Odrzucono wniosek o {LeaveTypeName} pracownika {RequesterFullName} z dnia {LeaveRequestCreatedAt}";
}
