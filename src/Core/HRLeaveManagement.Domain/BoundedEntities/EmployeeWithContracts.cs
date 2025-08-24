using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.RuleContracts;

namespace HRLeaveManagement.Domain.BoundedEntities;

public class EmployeeWithContracts(Employee employee)
{
    private Employee Employee => employee;
    public IReadOnlyList<EmployeeContract> EmployeeContracts => Employee.EmployeeContracts;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="contractType"></param>
    /// <param name="contractDetails"></param>
    /// <param name="startedAt"></param>
    /// <param name="expiredAt"></param>
    /// <param name="employeeDocuments"></param>
    /// <returns></returns>
    public EmployeeContract AddContract(ContractType contractType,
                                        string? contractDetails,
                                        DateOnly startedAt,
                                        DateOnly? expiredAt = null,
                                        IEnumerable<EmployeeDocument>? employeeDocuments = null)
        =>
            Employee.AddContract(contractType, contractDetails, startedAt, expiredAt, employeeDocuments);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="employeeDocumentRuleSet"></param>
    /// <param name="employeeContractPayloads"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<EmployeeContract>> AddManyContractsAsync(
        IEmployeeDocumentRuleSet employeeDocumentRuleSet,
        IEnumerable<EmployeeContractPayload> employeeContractPayloads,
        CancellationToken cancellationToken = default
    )
        => await Employee.AddManyContractsAsync(employeeDocumentRuleSet, employeeContractPayloads, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="contractToRemove"></param>
    public void RemoveContract(EmployeeContract contractToRemove) => Employee.RemoveContract(contractToRemove);
}
