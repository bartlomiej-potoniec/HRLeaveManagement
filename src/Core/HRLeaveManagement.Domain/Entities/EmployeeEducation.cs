using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Entities;

public class EmployeeEducation
{
    public int Id { get; set; }

    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public EducationType EducationType { get; set; }
    public string EducationDetails { get; set; }

    public DateOnly EnrolledAt { get; set; }
    public DateOnly? GraduatedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

    private EmployeeEducation() {}


    #region Domain_Factory_Methods

    public static EmployeeEducation Create(Employee employee,
                                           EducationType educationType,
                                           string educationDetails,
                                           DateOnly enrolledAt,
                                           DateOnly? graduatedAt)
        => new()
        {
            Employee = employee,
            EducationType = educationType,
            EducationDetails = educationDetails,
            EnrolledAt = enrolledAt,
            GraduatedAt = graduatedAt,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

    public static EmployeeEducation Create(Employee employee,
                                           EducationType educationType,
                                           string educationDetails,
                                           DateTime enrolledAt,
                                           DateTime? graduatedAt)
        => new()
        {
            Employee = employee,
            EducationType = educationType,
            EducationDetails = educationDetails,
            EnrolledAt = DateOnly.FromDateTime(enrolledAt),
            GraduatedAt = graduatedAt.HasValue 
                ? DateOnly.FromDateTime(graduatedAt.Value) 
                : null,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

    public static EmployeeEducation Create(Guid employeeId,
                                           EducationType educationType,
                                           string educationDetails,
                                           DateOnly enrolledAt,
                                           DateOnly? graduatedAt)
        => new()
        {
            EmployeeId = employeeId,
            EducationType = educationType,
            EducationDetails = educationDetails,
            EnrolledAt = enrolledAt,
            GraduatedAt = graduatedAt,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

    public static EmployeeEducation Create(Guid employeeId,
                                           EducationType educationType,
                                           string educationDetails,
                                           DateTime enrolledAt,
                                           DateTime? graduatedAt)
        => new()
        {
            EmployeeId = employeeId,
            EducationType = educationType,
            EducationDetails = educationDetails,
            EnrolledAt = DateOnly.FromDateTime(enrolledAt),
            GraduatedAt = graduatedAt.HasValue 
                ? DateOnly.FromDateTime(graduatedAt.Value) 
                : null,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

    public static void Update(EmployeeEducation entity,
                              EducationType educationType,
                              string educationDetails,
                              DateOnly enrolledAt,
                              DateOnly? graduatedAt)
    {
        entity.EducationType = educationType;
        entity.EducationDetails = educationDetails;
        entity.EnrolledAt = enrolledAt;
        entity.GraduatedAt = graduatedAt;
        entity.ModifiedAt = DateTime.UtcNow;
    }

    public static void Update(EmployeeEducation entity,
                              EducationType educationType,
                              string educationDetails,
                              DateTime enrolledAt,
                              DateTime? graduatedAt)
    {
        entity.EducationType = educationType;
        entity.EducationDetails = educationDetails;
        entity.EnrolledAt = DateOnly.FromDateTime(enrolledAt);
        entity.GraduatedAt = graduatedAt.HasValue
            ? DateOnly.FromDateTime(graduatedAt.Value)
            : null;
        entity.ModifiedAt = DateTime.UtcNow;
    }

    #endregion
}