using HRLeaveManagement.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRLeaveManagement.Domain;

public class LeaveRequest : BaseEntity
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int LeaveTypeId { get; set; }

    [ForeignKey("LeaveTypeId")]
    public LeaveType? LeaveType { get; set; }

    public DateTime RequestedDate { get; set; }
    public string? RequestedComments { get; set; }

    public bool? Approved { get; set; }
    public bool Canceled { get; set; }

    public string RequestingEmployeeId { get; set; } = string.Empty;
}
