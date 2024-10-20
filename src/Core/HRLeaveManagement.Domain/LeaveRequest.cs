using HRLeaveManagement.Domain.Common;

namespace HRLeaveManagement.Domain;

public class LeaveRequest : BaseEntity
{
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }

    public LeaveType? LeaveType { get; set; }
    public int LeaveTypeId { get; set; }

    public DateTime RequestedDate { get; set; }
    public string? RequestComment { get; set; }

    public bool? IsApproved { get; set; }
    public bool IsCanceled { get; set; }

    public string RequestingEmployeeId { get; set; } = string.Empty;
}
