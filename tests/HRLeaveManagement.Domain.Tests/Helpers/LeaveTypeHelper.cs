using HRLeaveManagement.Domain.RuleContracts;

namespace HRLeaveManagement.Domain.Tests.Helpers;

internal class LeaveTypeHelper
{
    internal static async Task<LeaveType> CreateLeaveTypeAsync(string name,
                                                               decimal paidFraction = 1.0M,
                                                               string? description = null)
    {
        Mock<ILeaveTypeRuleSet> leaveTypeRuleSetMock = new();
        leaveTypeRuleSetMock
            .Setup(rule => rule.IsNameUniqueAsync(name, CancellationToken.None))
            .ReturnsAsync(true);

        LeaveType leaveType = await LeaveType.CreateAsync(leaveTypeRuleSetMock.Object, name, paidFraction, description);

        return leaveType;
    }
}
