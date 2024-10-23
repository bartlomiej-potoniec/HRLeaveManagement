namespace HRLeaveManagement.Domain;

public class Section
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; }
}
