using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.BoundedEntities;

public class LeaveEvaluationContext(EmployeeWithAllInfo employee)
{
    public Employee Employee => employee.Employee;
    public GenderType EmployeeGender => Employee.Gender;

    public EmployeeContract? CurrentContract => Employee.EmployeeContracts
        .FirstOrDefault(c => 
            c.StartedAt <= DateOnly.FromDateTime(DateTime.UtcNow) && 
            (c.ExpiredAt is null || c.ExpiredAt >= DateOnly.FromDateTime(DateTime.UtcNow)));

    public IReadOnlyList<EmployeeContract> EmployeeContracts => Employee.EmployeeContracts;

    public IReadOnlyList<EmployeeExperience> EmployeeExperiences => Employee.EmployeeExperiences;
    
    public IReadOnlyList<EmployeeEducation> EmployeeEducations => Employee.EmployeeEducations;
}
