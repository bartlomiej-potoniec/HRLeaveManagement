namespace HRLeaveManagement.Domain;

public class LeaveAllocation
{
    public int Id { get; set; }
    public Guid EmployeeId { get; set; }

    public int LeaveTypeId { get; set; }
    public LeaveType? LeaveType { get; set; }

    public int Period { get; set; }

    public int? NumberOfDays { get; set; }
    public int? UsedDays { get; set; }
    public int? RemainingDays { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}
