namespace HRLeaveManagement.Domain;

public class TimeRegister
{
    public int Id { get; set; }
    public Guid EmployeeId { get; set; }
    public DateOnly Date { get; set; }

    public TimeOnly WorkStartedAt { get; set; }
    public TimeOnly WorkEndedAt { get; set; }
    public TimeSpan TotalWorkTime { get; set; }

    public TimeOnly? BreakStartedAt { get; set; }
    public TimeOnly? BreakEndedAt { get; set; }
    public TimeSpan? TotalBreakTime { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}