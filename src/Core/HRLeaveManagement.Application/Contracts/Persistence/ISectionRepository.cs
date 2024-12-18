using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.Contracts.Persistence;

public interface ISectionRepository
{
    Task<IEnumerable<Section>> GetAllAsync();
    Task<IEnumerable<Section>> GetAllByDepartmentIdAsync(int departmentId);
    Task<Section?> GetByIdAsync(int id);

    Task CreateAsync(Section section);
    Task UpdateAsync(Section section);
}
