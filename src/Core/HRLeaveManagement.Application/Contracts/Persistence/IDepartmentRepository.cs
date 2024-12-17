using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.Contracts.Persistence;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(int departmentId);

    Task CreateAsync(Department department);
    Task UpdateAsync(Department department);
}
