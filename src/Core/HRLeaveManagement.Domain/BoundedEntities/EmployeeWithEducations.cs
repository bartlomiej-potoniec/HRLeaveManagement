using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.RuleContracts;

namespace HRLeaveManagement.Domain.BoundedEntities;

public class EmployeeWithEducations(Employee employee)
{
    public Employee Employee => employee;
    public IReadOnlyList<EmployeeEducation> EmployeeEducations => Employee.EmployeeEducations;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="educationType"></param>
    /// <param name="institutionName"></param>
    /// <param name="enrolledAt"></param>
    /// <param name="graduatedAt"></param>
    /// <param name="educationDetails"></param>
    /// <param name="employeeDocuments"></param>
    /// <returns></returns>
    public EmployeeEducation AddEducation(EducationType educationType,
                                          string institutionName,
                                          DateOnly enrolledAt,
                                          DateOnly? graduatedAt,
                                          string? educationDetails = null,
                                          IEnumerable<EmployeeDocument>? employeeDocuments = null) 
        => 
            Employee.AddEducation(educationType, institutionName, enrolledAt, graduatedAt, educationDetails, employeeDocuments);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="employeeDocumentRuleSet"></param>
    /// <param name="employeeEducationPayloads"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<EmployeeEducation>> AddManyEducationsAsync(
        IEmployeeDocumentRuleSet employeeDocumentRuleSet,
        IEnumerable<EmployeeEducationPayload> employeeEducationPayloads,
        CancellationToken cancellationToken = default
    ) 
        => await Employee.AddManyEducationsAsync(employeeDocumentRuleSet, employeeEducationPayloads, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="educationToRemove"></param>
    public void RemoveEducation(EmployeeEducation educationToRemove) => Employee.RemoveEducation(educationToRemove);
}
