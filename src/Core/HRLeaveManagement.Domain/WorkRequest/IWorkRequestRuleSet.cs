namespace HRLeaveManagement.Domain.WorkRequest;

public interface IWorkRequestRuleSet
{
    Task<bool> IsRequestApproverSuperiorOfEmployeeAsync(Employee.Employee approver,
                                                        Employee.Employee requestingEmployee,
                                                        CancellationToken cancellationToken);
}
