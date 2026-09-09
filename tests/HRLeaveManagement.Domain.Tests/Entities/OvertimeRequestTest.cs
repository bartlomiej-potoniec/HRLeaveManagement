using HRLeaveManagement.Domain.Tests.Helpers;
using HRLeaveManagement.Domain.WorkRequest;
using HRLeaveManagement.Domain.WorkRequest.OvertimeRequest;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class OvertimeRequestTest
{
    [Fact]
    public void Create_ForGivenParams_ReturnsNewInstance()
    {
        // Arrange
        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();
        DateOnly startedAt = new(2024, 11, 11);
        DateOnly endedAt = new(2024, 11, 13);
        string approverComment = "Comment for employee's overtime";
        string purposeDescription = "Overtime because of work";

        Mock<IWorkRequestRuleSet> workRequestRuleSetMock = WorkRequestTestHelper.CreateWorkRequestRuleSetMock();
        WorkRequestTestHelper.SetupIsRequestApproverSuperiorOfEmployeeAsyncToReturnResult(workRequestRuleSetMock, isRuleFailed: false);

        // Act
        var delegationRequest = OvertimeRequest.Create(
            workRequestRuleSetMock.Object,
            requestingEmployee,
            startedAt,
            endedAt,
            approver,
            approverComment,
            purposeDescription
        );

        // Assert
        delegationRequest
            .Should()
            .BeOfType<OvertimeRequest>();
    }
}
