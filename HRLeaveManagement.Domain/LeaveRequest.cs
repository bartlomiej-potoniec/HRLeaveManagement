using HRLeaveManagement.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRLeaveManagement.Domain;

public class LeaveRequest : BaseEntity
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public LeaveType? LeaveType { get; set; }
    public int LeaveTypeId { get; set; }

    public DateTime RequestedDate { get; set; }
    public string? RequestComment { get; set; }

    public bool? IsApproved { get; set; }
    public bool IsCanceled { get; set; }

    public string RequestingEmployeeId { get; set; } = string.Empty;
}
