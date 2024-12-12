using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.Contracts.Persistence;

public interface ILeaveTypeRepository
{
    Task<IEnumerable<LeaveType>> GetAllAsync();
    Task<LeaveType> GetByIdAsync(int id);
    Task<int> CreateAsync(LeaveType leaveType);
    Task UpdateAsync(LeaveType leaveType);
    Task DeleteAsync(LeaveType leaveType);
    Task<bool> IsLeaveTypeUnique(string name);
}
