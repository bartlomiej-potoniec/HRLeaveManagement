using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.Contracts.Persistence;

public interface ISectionRepository
{
    Task<IEnumerable<Section>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Section>> GetAllByDepartmentIdAsync(int departmentId, CancellationToken cancellationToken = default);
    Task<Section?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task CreateAsync(Section section, CancellationToken cancellationToken = default);
    Task UpdateAsync(Section section, CancellationToken cancellationToken = default);
}
