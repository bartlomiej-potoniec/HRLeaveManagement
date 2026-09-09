using HRLeaveManagement.Domain.Tests.Helpers;
using HRLeaveManagement.Domain.WorkRequest;
using HRLeaveManagement.Domain.WorkRequest.ExtraRemoteWorkRequest;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class ExtraRemoteWorkRequestTest
{
    [Fact]
    public void Create_ForGivenParams_ReturnsNewInstance()
    {
        // Arrange
        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();
        DateOnly startedAt = new(2024, 11, 11);
        DateOnly endedAt = new(2024, 11, 13);
        string approverComment = "Comment for extra-remote-work request";
        string reasonDescription = "extra-remote-work because of work";

        Mock<IWorkRequestRuleSet> workRequestRuleSetMock = WorkRequestTestHelper.CreateWorkRequestRuleSetMock();
        WorkRequestTestHelper.SetupIsRequestApproverSuperiorOfEmployeeAsyncToReturnResult(workRequestRuleSetMock, isRuleFailed: false);

        // Act
        var delegationRequest = ExtraRemoteWorkRequest.Create(
            workRequestRuleSetMock.Object,
            requestingEmployee,
            startedAt,
            endedAt,
            approver,
            approverComment,
            reasonDescription
        );

        // Assert
        delegationRequest
            .Should()
            .BeOfType<ExtraRemoteWorkRequest>();
    }
}
