using DomainEmployee = HRLeaveManagement.Domain.Employee.Employee;
using HRLeaveManagement.Domain.Leave.LeaveRequest;
using Microsoft.EntityFrameworkCore;
using HRLeaveManagement.Domain.Leave.LeaveRequest.CheckerContracts;

namespace HRLeaveManagement.Persistence.Leave.LeaveRequest.Checkers;

public sealed class NoAnotherLeaveRequestExistsInPeriodChecker(ApplicationDbContext dbContext)
    : INoAnotherLeaveRequestExistsInPeriodChecker
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<bool> IsEligible(DomainEmployee requestingEmployee, DateOnly leaveStartedAt, DateOnly leaveEndedAt)
    {
        var isAnyExistingEmployeeRequestForGivenPeriod = await _dbContext.LeaveRequests
            .AnyAsync(req =>
                req.RequestingEmployeeId == requestingEmployee.Id &&
                (req.Status == RequestStatus.Approved || req.Status == RequestStatus.Pending) &&
                req.StartedAt <= leaveEndedAt &&
                req.EndedAt >= leaveStartedAt);

        return !isAnyExistingEmployeeRequestForGivenPeriod;
    }
}
