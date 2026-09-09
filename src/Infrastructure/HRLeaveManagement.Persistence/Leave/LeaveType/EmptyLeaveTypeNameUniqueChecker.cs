using HRLeaveManagement.Domain.Leave.LeaveType;

namespace HRLeaveManagement.Persistence.Leave.LeaveType;

public sealed class EmptyLeaveTypeNameUniqueChecker : ILeaveTypeNameUniqueChecker
{
    public static readonly EmptyLeaveTypeNameUniqueChecker Instance = new();
    private EmptyLeaveTypeNameUniqueChecker() {}

    public Task<bool> IsEligible(string name, CancellationToken cancellationToken) => Task.FromResult(true);
}
