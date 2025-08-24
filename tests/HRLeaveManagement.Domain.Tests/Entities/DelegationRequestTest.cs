using HRLeaveManagement.Domain.RuleContracts;
using HRLeaveManagement.Domain.Tests.Helpers;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class DelegationRequestTest
{
    [Fact]
    public void Create_ForGivenRequestingEmployee_ThrowsArgumentException_WhenRequestingEmployeeIsSubstitutor()
    {
        // Arrange
        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();
        Employee substitutor = EmployeeHelper.CreateEmployee();
        DateOnly startedAt = new(2024, 11, 11);
        DateOnly endedAt = new(2024, 11, 13);
        string destinationCountry = "Germany";
        string meansOfTransport = "By car";
        decimal cashAdvance = 150.5M;

        Mock<IWorkRequestRuleSet> workRequestRuleSetMock = WorkRequestTestHelper.CreateWorkRequestRuleSetMock();

        // Act
        Action result = async () => await DelegationRequest.Create(
            workRequestRuleSetMock.Object,
            requestingEmployee,
            startedAt,
            endedAt,
            approver,
            substitutor,
            destinationCountry,
            meansOfTransport,
            cashAdvance
        );

        string expectedExceptionMessage = "Requesting employee cannot be their own substitutor";

        // Assert
        result
            .Should()
            .Throw<ArgumentException>(expectedExceptionMessage)
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-1)]
    [InlineData(-12)]
    public void Create_ForGivenCashAdvance_ThrowsArgumentException_WhenCashAdvanceIsFewerThanZero(decimal cashAdvance)
    {
        // Arrange
        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        DateOnly startedAt = new(2024, 11, 11);
        DateOnly endedAt = new(2024, 11, 13);
        Employee approver = EmployeeHelper.CreateEmployee();
        Employee substitutor = EmployeeHelper.CreateEmployee();
        string destinationCountry = "Germany";
        string meansOfTransport = "By car";

        Mock<IWorkRequestRuleSet> workRequestRuleSetMock = WorkRequestTestHelper.CreateWorkRequestRuleSetMock();

        // Act
        Action result = async () => await DelegationRequest.Create(
            workRequestRuleSetMock.Object,
            requestingEmployee,
            startedAt,
            endedAt,
            approver,
            substitutor,
            destinationCountry,
            meansOfTransport,
            cashAdvance
        );

        string expectedExceptionMessage = "Cash advance must be greater than 0";

        // Assert
        result
            .Should()
            .Throw<ArgumentException>(expectedExceptionMessage)
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Create_ForGivenParams_ReturnsNewInstance()
    {
        // Arrange
        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        DateOnly startedAt = new(2024, 11, 11);
        DateOnly endedAt = new(2024, 11, 13);
        Employee approver = EmployeeHelper.CreateEmployee();
        Employee substitutor = EmployeeHelper.CreateEmployee();
        string destinationCountry = "Germany";
        string meansOfTransport = "By car";
        decimal cashAdvance = 150.5M;

        Mock<IWorkRequestRuleSet> workRequestRuleSetMock = WorkRequestTestHelper.CreateWorkRequestRuleSetMock();
        WorkRequestTestHelper.SetupIsRequestApproverSuperiorOfEmployeeAsyncToReturnResult(workRequestRuleSetMock, isRuleFailed: false);

        // Act
        var delegationRequest = await DelegationRequest.Create(
            workRequestRuleSetMock.Object,
            requestingEmployee,
            startedAt,
            endedAt,
            approver,
            substitutor,
            destinationCountry,
            meansOfTransport,
            cashAdvance
        );

        // Assert
        delegationRequest
            .Should()
            .BeOfType<DelegationRequest>();
    }

}
