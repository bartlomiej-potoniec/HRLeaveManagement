using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.Contracts.Persistence;

public interface ISectionRepository
{
    Task<Section?> GetByIdAsync(int id);
    Task<IEnumerable<Section>> GetAllByDepartmentIdAsync(int departmentId);
}
