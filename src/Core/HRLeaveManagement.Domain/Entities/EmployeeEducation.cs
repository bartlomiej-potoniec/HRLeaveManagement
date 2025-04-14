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
    {
        if (employee is null)
        {
            throw new ArgumentException("Employee must be included");
        }

        ValidateBaseRules(educationType, educationDetails, enrolledAt, graduatedAt);

        return new()
        {
            Employee = employee,
            EducationType = educationType,
            EducationDetails = educationDetails,
            EnrolledAt = enrolledAt,
            GraduatedAt = graduatedAt,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
    }

    public static EmployeeEducation Create(Employee employee,
                                           EducationType educationType,
                                           string educationDetails,
                                           DateTime enrolledAt,
                                           DateTime? graduatedAt)
    {
        if (employee is null)
        {
            throw new ArgumentException("Employee must be included");
        }

        ValidateBaseRules(educationType, educationDetails, enrolledAt, graduatedAt);

        return new()
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
    }

    public static void Update(EmployeeEducation employeeEducation,
                              EducationType educationType,
                              string educationDetails,
                              DateOnly enrolledAt,
                              DateOnly? graduatedAt)
    {
        if (employeeEducation is null)
        {
            throw new ArgumentException("Employee must be included");
        }

        ValidateBaseRules(educationType, educationDetails, enrolledAt, graduatedAt);

        employeeEducation.EducationType = educationType;
        employeeEducation.EducationDetails = educationDetails;
        employeeEducation.EnrolledAt = enrolledAt;
        employeeEducation.GraduatedAt = graduatedAt;
        employeeEducation.ModifiedAt = DateTime.UtcNow;
    }

    public static void Update(EmployeeEducation employeeEducation,
                              EducationType educationType,
                              string educationDetails,
                              DateTime enrolledAt,
                              DateTime? graduatedAt)
    {
        if (employeeEducation is null)
        {
            throw new ArgumentException("Employee must be included");
        }

        ValidateBaseRules(educationType, educationDetails, enrolledAt, graduatedAt);

        employeeEducation.EducationType = educationType;
        employeeEducation.EducationDetails = educationDetails;
        employeeEducation.EnrolledAt = DateOnly.FromDateTime(enrolledAt);
        employeeEducation.GraduatedAt = graduatedAt.HasValue
            ? DateOnly.FromDateTime(graduatedAt.Value)
            : null;
        employeeEducation.ModifiedAt = DateTime.UtcNow;
    }

    private static void ValidateBaseRules(EducationType educationType,
                                          string educationDetails,
                                          DateOnly enrolledAt,
                                          DateOnly? graduatedAt)
    {
        if (string.IsNullOrEmpty(educationDetails))
        {
            throw new ArgumentException("Education details for employee cannot be empty");
        }

        if (graduatedAt.HasValue && graduatedAt < enrolledAt)
        {
            throw new InvalidOperationException("Education graduation date must be greater than enroll date");
        }
    }

    private static void ValidateBaseRules(EducationType educationType,
                                          string educationDetails,
                                          DateTime enrolledAt,
                                          DateTime? graduatedAt)
    {
        if (string.IsNullOrEmpty(educationDetails))
        {
            throw new ArgumentException("Education details for employee cannot be empty");
        }

        if (graduatedAt.HasValue && graduatedAt < enrolledAt)
        {
            throw new InvalidOperationException("Education graduation date must be greater than enroll date");
        }
    }

    #endregion
}