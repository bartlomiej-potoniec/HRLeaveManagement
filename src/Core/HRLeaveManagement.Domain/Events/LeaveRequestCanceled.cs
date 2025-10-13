using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Events;

public sealed class LeaveRequestCanceled(LeaveRequest leaveRequest) : IEntityEvent
{
    public int LeaveRequestId => leaveRequest.Id;
    public RequestStatus RequestStatus => leaveRequest.Status;
    public DateTime LeaveRequestCreatedAt => leaveRequest.CreatedAt;
    public int LeaveRequestCurrentlyUsedDays => leaveRequest.TotalDays;

    public string LeaveTypeName => leaveRequest.LeaveType.Name;
    
    public Guid RequesterId => leaveRequest.RequestingEmployeeId;
    public string RequesterFullName
        => $"{ leaveRequest.RequestingEmployee.FirstName } { leaveRequest.RequestingEmployee.LastName }";

    public DateTime OccurredOn => DateTime.UtcNow;
    public string Content => $"Anulowano wniosek o { LeaveTypeName } pracownika { RequesterFullName } z dnia { LeaveRequestCreatedAt }";
}
