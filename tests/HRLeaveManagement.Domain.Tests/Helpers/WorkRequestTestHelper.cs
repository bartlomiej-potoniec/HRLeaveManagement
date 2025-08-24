using HRLeaveManagement.Domain.RuleContracts;

namespace HRLeaveManagement.Domain.Tests.Helpers;

internal class WorkRequestTestHelper : WorkRequest
{
    internal async Task InitializeBase(Employee requestingEmployee,
                                       DateOnly startedAt,
                                       DateOnly endedAt,
                                       Employee approver,
                                       Mock<IWorkRequestRuleSet> workRequestRuleSet,
                                       string? approverComment = null)
        => 
            await base.InitializeBase<WorkRequest>(
                requestingEmployee,
                startedAt,
                endedAt,
                approver,
                approverComment,
                workRequestRuleSet.Object
            );

    internal static Mock<IWorkRequestRuleSet> CreateWorkRequestRuleSetMock() => new();

    internal static void SetupIsRequestApproverSuperiorOfEmployeeAsyncToReturnResult(Mock<IWorkRequestRuleSet> workRequestRuleSetMock,
                                                                                     bool isRuleFailed)
        => workRequestRuleSetMock
            .Setup(rule => rule
                .IsRequestApproverSuperiorOfEmployeeAsync(It.IsAny<Employee>(), It.IsAny<Employee>(), CancellationToken.None))
            .ReturnsAsync(isRuleFailed);
}
