using HRLeaveManagement.Domain.Leave.LeaveRequest.CheckerContracts;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Leave.LeaveRequest.Checkers;

public sealed class LeaveRequesterHasEnoughRemainingDaysChecker(ApplicationDbContext dbContext)
    : ILeaveRequesterHasEnoughRemainingDaysChecker
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<bool> IsEligible(Guid requestingEmployeeId,
                                       int requestingDays,
                                       int currentYear,
                                       CancellationToken cancellationToken)
    {
        var employeeCurrentYearAllocation = await _dbContext.LeaveAllocations
            .AsNoTracking()
            .FirstOrDefaultAsync(allocation => 
                allocation.EmployeeId == requestingEmployeeId &&
                allocation.Year == currentYear, 
                cancellationToken: cancellationToken);

        int? remainingDaysInAllocation = employeeCurrentYearAllocation?.RemainingDays;
        var isLeaveLimited = remainingDaysInAllocation is not null;

        return !isLeaveLimited || requestingDays <= remainingDaysInAllocation;
    }
}
