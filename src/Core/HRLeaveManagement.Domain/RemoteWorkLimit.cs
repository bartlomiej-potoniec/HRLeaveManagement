namespace HRLeaveManagement.Domain;

public class RemoteWorkLimit
{
    public int Id { get; set; }
    public Guid EmployeeId { get; set; }
    public int Year { get; set; }
    public int MaxDays { get; set; }
    public int UsedDays { get; set; }
    public int RemainingDays { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}
