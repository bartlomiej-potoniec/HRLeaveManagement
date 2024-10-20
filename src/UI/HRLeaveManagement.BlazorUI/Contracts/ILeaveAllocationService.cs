using HRLeaveManagement.BlazorUI.Models;

namespace HRLeaveManagement.BlazorUI.Contracts;

public interface ILeaveAllocationService
{
    Task<Response<Guid>> CreateLeaveAllocations(int leaveTypeId);
}
