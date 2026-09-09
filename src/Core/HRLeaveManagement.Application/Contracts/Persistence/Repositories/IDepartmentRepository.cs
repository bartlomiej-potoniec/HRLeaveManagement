using HRLeaveManagement.Domain.Department;

namespace HRLeaveManagement.Application.Contracts.Persistence.Repositories;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Department?> GetByIdAsync(int departmentId, CancellationToken cancellationToken = default);
    Task<Department?> GetWithDetailsById(int departmentId, CancellationToken cancellationToken = default);
    Task CreateAsync(Department department, CancellationToken cancellationToken = default);
    Task UpdateAsync(Department department, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
