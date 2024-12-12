using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.Contracts.Persistence;

public interface ILeaveRequestRepository
{
    Task<LeaveRequest?> GetByIdAsync(int id);
    Task UpdateAsync(LeaveRequest leaveRequest);
    Task<LeaveRequest?> GetLeaveRequestWithDetailsByIdAsync(int id);
    Task<IReadOnlyList<LeaveRequest>> GetAllLeaveRequestsWithDetailsAsync();
    Task<IReadOnlyList<LeaveRequest>> GetEmployeeLeaveRequestsWithDetailsAsync(Guid employeeId);
    Task DeleteAsync(LeaveRequest leaveRequest);
}
