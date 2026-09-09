using HRLeaveManagement.Domain.Employee.ExternalContracts;

namespace HRLeaveManagement.Domain.Leave;

public interface ILeavePolicy
{
    string Code { get; }
    bool IsEligibleFor(EmployeeLeaveInformation employee);
    int? CalculateDaysFor(EmployeeLeaveInformation employee);
}
