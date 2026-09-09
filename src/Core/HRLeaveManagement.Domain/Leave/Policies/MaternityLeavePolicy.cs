using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Employee;
using HRLeaveManagement.Domain.Employee.Contract;
using HRLeaveManagement.Domain.Leave;

namespace HRLeaveManagement.Domain.Policies.Leaves;

public class MaternityLeavePolicy : ILeavePolicy
{
    public string Code => "MATERNITY";

    public int? CalculateDaysFor(LeaveEvaluationContext context) => null;

    public bool IsEligibleFor(Employee.Employee employee)
        => employee.CurrentContract is not null &&
            employee.Gender is GenderType.Female &&
            (employee.CurrentContract.ContractType is ContractType.Employment ||
             employee.CurrentContract.ContractType is ContractType.B2B ||
             employee.CurrentContract.ContractType is ContractType.Other);
}
