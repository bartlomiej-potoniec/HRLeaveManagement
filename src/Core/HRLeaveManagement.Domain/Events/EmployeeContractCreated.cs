using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.Events;

public sealed class EmployeeContractCreated(EmployeeContract contract) : IEntityEvent
{
    public string ContractType => contract.ContractType.ToString();
    public string EmployeeFullName => $"{ contract.Employee.FirstName } { contract.Employee.LastName }";

    public DateTime OccurredOn => DateTime.UtcNow;
    public string Content => $"Utworzono nową umowę: { ContractType } dla pracownika { EmployeeFullName }";
}
