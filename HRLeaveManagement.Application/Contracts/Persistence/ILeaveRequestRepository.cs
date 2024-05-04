using HRLeaveManagement.Domain;

namespace HRLeaveManagement.Application.Contracts.Persistence;

public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest> 
{
    Task<LeaveRequest?> GetLeaveRequestWithDetailsByIdAsync(int id);
    Task<IReadOnlyList<LeaveRequest>> GetAllLeaveRequestsWithDetailsAsync();
    Task<IReadOnlyList<LeaveRequest>> GetUserLeaveRequestsWithDetailsAsync(string userId);
}
