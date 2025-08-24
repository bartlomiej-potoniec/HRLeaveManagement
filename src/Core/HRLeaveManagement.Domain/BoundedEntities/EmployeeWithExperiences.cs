using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.RuleContracts;

namespace HRLeaveManagement.Domain.BoundedEntities;

public class EmployeeWithExperiences(Employee employee)
{
    public Employee Employee => employee;
    public IReadOnlyList<EmployeeExperience> EmployeeExperiences => Employee.EmployeeExperiences;

    public EmployeeExperience AddExperience(ContractType contractType,
                                            string previousCompanyName,
                                            string position,
                                            DateOnly employedFrom,
                                            DateOnly employedTo,
                                            string? experienceDetails = null,
                                            IEnumerable<EmployeeDocument>? employeeDocuments = null)
        => 
            Employee.AddExperience(contractType, previousCompanyName, position, employedFrom, employedTo, experienceDetails, employeeDocuments);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="employeeDocumentRuleSet"></param>
    /// <param name="employeeExperiencePayload"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<EmployeeExperience>> AddManyExperiencesAsync(
        IEmployeeDocumentRuleSet employeeDocumentRuleSet,
        IEnumerable<EmployeeExperiencePayload> employeeExperiencePayload,
        CancellationToken cancellationToken = default
    )
        => await Employee.AddManyExperiencesAsync(employeeDocumentRuleSet, employeeExperiencePayload, cancellationToken);

    public void RemoveExperience(EmployeeExperience experienceToRemove) => Employee.RemoveExperience(experienceToRemove);
}
