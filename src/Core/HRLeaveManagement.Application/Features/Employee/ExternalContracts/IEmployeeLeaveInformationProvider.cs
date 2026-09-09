using HRLeaveManagement.Domain.Employee.ExternalContracts;

namespace HRLeaveManagement.Application.Features.Employee.ExternalContracts;

public interface IEmployeeLeaveInformationProvider
{
    Task<EmployeeLeaveInformation?> GetAsync(Guid employeeId, CancellationToken cancellationToken = default);
}
