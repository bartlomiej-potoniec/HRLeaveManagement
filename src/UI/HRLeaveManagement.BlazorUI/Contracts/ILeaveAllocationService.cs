using HRLeaveManagement.BlazorUI.Models;

namespace HRLeaveManagement.BlazorUI.Contracts;

public interface ILeaveAllocationService
{
    Task<Response> CreateLeaveAllocations(int leaveTypeId);
}
