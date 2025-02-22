using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.Contracts.Persistence;

public interface ILeaveRequestRepository
{
    Task<LeaveRequest?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateAsync(LeaveRequest leaveRequest, CancellationToken cancellationToken = default);
    Task<LeaveRequest?> GetLeaveRequestWithDetailsByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LeaveRequest>> GetAllLeaveRequestsWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LeaveRequest>> GetEmployeeLeaveRequestsWithDetailsAsync(Guid employeeId,
                                                                               CancellationToken cancellationToken = default);
    Task DeleteAsync(LeaveRequest leaveRequest, CancellationToken cancellationToken = default);
}
