using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.RuleContracts;
using HRLeaveManagement.Domain.Tests.Helpers;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class WorkRequestTest
{
    [Fact]
    public void InitializeBase_ForGivenEmployees_ThrowsArgumentException_WhenRequestingEmployeeIsApprover()
    {
        // Arrange
        DateOnly startedAt = new(2024, 9, 1);
        DateOnly endedAt = new(2024, 10, 1);

        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee approver = requestingEmployee;

        var workRequest = new WorkRequestTestHelper();

        Mock<IWorkRequestRuleSet> workRequestRuleSetMock = WorkRequestTestHelper.CreateWorkRequestRuleSetMock();
        WorkRequestTestHelper
            .SetupIsRequestApproverSuperiorOfEmployeeAsyncToReturnResult(workRequestRuleSetMock, isRuleFailed: false);

        string expectedExceptionMessage = "Requesting employee cannot be their own approver";

        // Act
        Action result = async () => 
            await workRequest.InitializeBase(requestingEmployee, startedAt, endedAt, approver, workRequestRuleSetMock);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void InitializeBase_ForGivenEmployees_ThrowsInvalidOperationException_WhenWorkRequestRuleFailed()
    {
        // Arrange
        DateOnly startedAt = new(2024, 9, 1);
        DateOnly endedAt = new(2024, 10, 1);

        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();

        var workRequest = new WorkRequestTestHelper();

        Mock<IWorkRequestRuleSet> workRequestRuleSetMock = WorkRequestTestHelper.CreateWorkRequestRuleSetMock();
        WorkRequestTestHelper
            .SetupIsRequestApproverSuperiorOfEmployeeAsyncToReturnResult(workRequestRuleSetMock, isRuleFailed: true);

        string expectedExceptionMessage = "Approver must be a superior of requesting employee";

        // Act
        Action result = async () =>
            await workRequest.InitializeBase(requestingEmployee, startedAt, endedAt, approver, workRequestRuleSetMock);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void InitializeBase_ForGivenRequestDates_ThrowsInvalidOperationException_WhenRequestStartedDateIsGreaterThanEndedDate()
    {
        // Arrange
        DateOnly startedAt = new(2024, 11, 1);
        DateOnly endedAt = new(2024, 9, 1);

        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();

        var workRequest = new WorkRequestTestHelper();

        Mock<IWorkRequestRuleSet> workRequestRuleSetMock = WorkRequestTestHelper.CreateWorkRequestRuleSetMock();
        WorkRequestTestHelper
            .SetupIsRequestApproverSuperiorOfEmployeeAsyncToReturnResult(workRequestRuleSetMock, isRuleFailed: false);

        string expectedExceptionMessage = "Request started date must be fewer than ended date";

        // Act
        Action result = async () =>
            await workRequest.InitializeBase(requestingEmployee, startedAt, endedAt, approver, workRequestRuleSetMock);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [MemberData(nameof(GetWorkRequestDateRanges))]
    public async Task InitializeBase_ForGivenRequestDates_ThrowsInvalidOperationException_WhenExistingRequestIsInGivenDateRange(DateOnly startedAt,
                                                                                                                                DateOnly endedAt)
    {
        // Arrange
        var requestingEmployeeWithWorkRequests = await EmployeeHelper.CreateEmployeeWithWorkRequestListAsync(
            (new DateOnly(2024, 11, 1), new DateOnly(2024, 11, 11)),
            (new DateOnly(2024, 11, 20), new DateOnly(2024, 12, 15)),
            (new DateOnly(2025, 1, 20), new DateOnly(2025, 2, 1)),
            (new DateOnly(2025, 2, 5), new DateOnly(2025, 2, 10))
        );

        Employee approver = EmployeeHelper.CreateEmployee();
        var workRequest = new WorkRequestTestHelper();

        Mock<IWorkRequestRuleSet> workRequestRuleSetMock = WorkRequestTestHelper.CreateWorkRequestRuleSetMock();
        WorkRequestTestHelper
            .SetupIsRequestApproverSuperiorOfEmployeeAsyncToReturnResult(workRequestRuleSetMock, isRuleFailed: false);

        string expectedExceptionMessage = "Given period for request is already included in another one";

        // Act
        Action result = async () =>
            await workRequest.InitializeBase(requestingEmployeeWithWorkRequests, startedAt, endedAt, approver, workRequestRuleSetMock);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Approve_ForGivenWorkRequester_ThrowsArgumentException_WhenRequesterIsNotApprover()
    {
        // Arrange
        DateOnly startedAt = new(2024, 9, 1);
        DateOnly endedAt = new(2024, 10, 1);

        Employee requester = EmployeeHelper.CreateEmployee();
        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();
        
        var workRequest = await CreateAndInitializeWorkRequestAsync(requestingEmployee, startedAt, endedAt, approver);

        string expectedExceptionMessage = "Only approver is allowed to approve the request";

        // Act 
        Action result = () => workRequest.Approve(requester);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }
    
    [Theory]
    [MemberData(nameof(GetWorkRequestActionsForUpdateStatus))]
    public async Task Approve_ForGivenWorkRequester_ThrowsInvalidOperationException_WhenRequestStatusIsNotPending(Action<WorkRequest, Employee> workRequestAction)
    {
        // Arrange
        DateOnly startedAt = new(2024, 9, 1);
        DateOnly endedAt = new(2024, 10, 1);

        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();

        var workRequest = await CreateAndInitializeWorkRequestAsync(requestingEmployee, startedAt, endedAt, approver);

        workRequestAction.Invoke(workRequest, approver);

        string expectedExceptionMessage = "It is not allowed to approve canceled or rejected request";

        // Act
        Action result = () => workRequest.Approve(approver);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Approve_ForGivenWorkRequester_ApprovesRequest()
    {
        // Arrange
        DateOnly startedAt = new(2024, 9, 1);
        DateOnly endedAt = new(2024, 10, 1);

        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();

        var workRequest = await CreateAndInitializeWorkRequestAsync(requestingEmployee, startedAt, endedAt, approver);

        // Act
        workRequest.Approve(approver);

        // Assert
        workRequest.Status
            .Should()
            .Be(RequestStatus.Approved);
    }

    [Fact]
    public async Task Reject_ForGivenWorkRequester_ThrowsArgumentException_WhenRequesterIsNotApprover()
    {
        // Arrange
        DateOnly startedAt = new(2024, 9, 1);
        DateOnly endedAt = new(2024, 10, 1);

        Employee requester = EmployeeHelper.CreateEmployee();
        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();

        var workRequest = await CreateAndInitializeWorkRequestAsync(requestingEmployee, startedAt, endedAt, approver);

        string expectedExceptionMessage = "Only approver is allowed to reject the request";

        // Act 
        Action result = () => workRequest.Reject(requester);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [MemberData(nameof(GetWorkRequestActionsForUpdateStatus))]
    public async Task Reject_ForGivenWorkRequester_ThrowsInvalidOperationException_WhenRequestStatusIsNotPending(Action<WorkRequest, Employee> workRequestAction)
    {
        // Arrange
        DateOnly startedAt = new(2024, 9, 1);
        DateOnly endedAt = new(2024, 10, 1);

        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();

        var workRequest = await CreateAndInitializeWorkRequestAsync(requestingEmployee, startedAt, endedAt, approver);

        workRequestAction.Invoke(workRequest, approver);

        string expectedExceptionMessage = "It is not allowed to reject canceled or approved request";

        // Act
        Action result = () => workRequest.Reject(approver);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Reject_ForGivenWorkRequester_ApprovesRequest()
    {
        // Arrange
        DateOnly startedAt = new(2024, 9, 1);
        DateOnly endedAt = new(2024, 10, 1);

        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();

        var workRequest = await CreateAndInitializeWorkRequestAsync(requestingEmployee, startedAt, endedAt, approver);

        // Act
        workRequest.Reject(approver);

        // Assert
        workRequest.Status
            .Should()
            .Be(RequestStatus.Approved);
    }

    [Fact]
    public async Task Cancel_ForRandomRequester_ThrowsArgumentException_WhenRequesterInNotRequestingEmployeeOrApprover()
    {
        // Arrange
        DateOnly startedAt = new(2024, 9, 1);
        DateOnly endedAt = new(2024, 10, 1);

        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();
        Employee randomRequester = EmployeeHelper.CreateEmployee();

        var workRequest = await CreateAndInitializeWorkRequestAsync(requestingEmployee, startedAt, endedAt, approver);

        string expectedExceptionMessage = "Only approver or requesting-employee are allowed to cancel the request";

        // Act
        Action result = () => workRequest.Cancel(randomRequester);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [MemberData(nameof(GetWorkRequestActionsForUpdateStatus))]
    public async Task Cancel_ForRequesterAsRequestingEmployee_ThrowsInvalidOperationException_WhenRequestStatusIsNotPending(Action<WorkRequest, Employee> workRequestAction)
    {
        // Arrange
        DateOnly startedAt = new(2024, 9, 1);
        DateOnly endedAt = new(2024, 10, 1);

        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();

        var workRequest = await CreateAndInitializeWorkRequestAsync(requestingEmployee, startedAt, endedAt, approver);

        workRequestAction.Invoke(workRequest, approver);

        string expectedExceptionMessage = "Only approver is allowed to cancel request after rejecting or approving";

        // Act
        Action result = () => workRequest.Cancel(requestingEmployee);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Cancel_ForRequesterAsRequestingEmployee_CancelsRequest_WhenRequestStatusIsPending()
    {
        // Arrange
        DateOnly startedAt = new(2024, 9, 1);
        DateOnly endedAt = new(2024, 10, 1);

        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();
        
        var workRequest = await CreateAndInitializeWorkRequestAsync(requestingEmployee, startedAt, endedAt, approver);

        // Act
        workRequest.Cancel(requestingEmployee);

        // Assert
        workRequest.Status
            .Should()
            .Be(RequestStatus.Canceled);
    }

    [Theory]
    [MemberData(nameof(GetWorkRequestActionsForCancel))]
    public async Task Cancel_ForRequesterAsApprover_CancelsRequest_WhenRequestStatusIsAny(Action<WorkRequest, Employee> workRequestAction)
    {
        // Arrange
        DateOnly startedAt = new(2024, 9, 1);
        DateOnly endedAt = new(2024, 10, 1);

        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();

        var workRequest = await CreateAndInitializeWorkRequestAsync(requestingEmployee, startedAt, endedAt, approver);

        workRequestAction.Invoke(workRequest, approver);

        // Act
        workRequest.Cancel(requestingEmployee);

        // Assert
        workRequest.Status
            .Should()
            .Be(RequestStatus.Canceled);
    }

    private static async Task<WorkRequestTestHelper> CreateAndInitializeWorkRequestAsync(Employee requestingEmployee,
                                                                                         DateOnly startedAt,
                                                                                         DateOnly endedAt,
                                                                                         Employee approver,
                                                                                         string? approverComment = null)
    {
        WorkRequestTestHelper workRequest = new();

        Mock<IWorkRequestRuleSet> workRequestRuleSetMock = WorkRequestTestHelper.CreateWorkRequestRuleSetMock();
        WorkRequestTestHelper
            .SetupIsRequestApproverSuperiorOfEmployeeAsyncToReturnResult(workRequestRuleSetMock, isRuleFailed: false);

        await workRequest.InitializeBase(requestingEmployee, startedAt, endedAt, approver, workRequestRuleSetMock, approverComment);

        return workRequest;
    }

    public static IEnumerable<object[]> GetWorkRequestActionsForUpdateStatus()
        => [
            [(WorkRequest workRequest, Employee requester) => workRequest.Approve(requester)],
            [(WorkRequest workRequest, Employee requester) => workRequest.Reject(requester)],
            [(WorkRequest workRequest, Employee requester) => workRequest.Cancel(requester)]
        ];

    public static IEnumerable<object[]> GetWorkRequestActionsForCancel()
        => [
            [(WorkRequest workRequest, Employee requester) => {}],
            [(WorkRequest workRequest, Employee requester) => workRequest.Approve(requester)],
            [(WorkRequest workRequest, Employee requester) => workRequest.Reject(requester)],
            [(WorkRequest workRequest, Employee requester) => workRequest.Cancel(requester)]
        ];

    public static IEnumerable<object[]> GetWorkRequestDateRanges()
        => [
            [new DateOnly(2024, 10, 15), new DateOnly(2024, 11, 5)],
            [new DateOnly(2024, 11, 7), new DateOnly(2024, 11, 25)],
            [new DateOnly(2024, 12, 1), new DateOnly(2025, 1, 1)],
            [new DateOnly(2025, 1, 1), new DateOnly(2025, 2, 7)],
            [new DateOnly(2025, 2, 8), new DateOnly(2025, 2, 13)],
            [new DateOnly(2024, 11, 1), new DateOnly(2024, 11, 9)],
            [new DateOnly(2025, 1, 22), new DateOnly(2025, 1, 27)],
            [new DateOnly(2024, 10, 20), new DateOnly(2025, 2, 20)],
        ];
}
