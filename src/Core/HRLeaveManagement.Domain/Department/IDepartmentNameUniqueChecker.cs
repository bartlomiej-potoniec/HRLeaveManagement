namespace HRLeaveManagement.Domain.Department;

public interface IDepartmentNameUniqueChecker
{
    Task<bool> IsEligible(string name, CancellationToken cancellationToken);
}
