using HRLeaveManagement.Domain.Employee.Education;

namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeEducationDetailsDTO
{
    public required int Id { get; init; }
    public required EducationType EducationType { get; init; }
    public required string EducationDetails { get; init; }
    public required DateTime EnrolledAt { get; init; }
    public DateTime? GraduatedAt { get; init; }

    public required DateTime CreatedAt { get; init; }
    public required DateTime ModifiedAt { get; init; }
}
