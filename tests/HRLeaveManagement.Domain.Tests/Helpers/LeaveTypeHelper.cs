using HRLeaveManagement.Domain.Leave.LeaveType;

namespace HRLeaveManagement.Domain.Tests.Helpers;

internal class LeaveTypeHelper
{
    internal static async Task<LeaveType> CreateLeaveTypeAsync(string name,
                                                               decimal paidFraction = 1.0M,
                                                               string? description = null)
    {
        Mock<ILeaveTypeNameUniqueChecker> leaveTypeRuleSetMock = new();
        leaveTypeRuleSetMock
            .Setup(rule => rule.IsEligible(name, CancellationToken.None))
            .ReturnsAsync(true);

        LeaveType leaveType = await LeaveType.CreateAsync(leaveTypeRuleSetMock.Object, name, paidFraction, description);

        return leaveType;
    }
}
