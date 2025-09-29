using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.RuleContracts;
using HRLeaveManagement.Domain.Tests.Helpers;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class LeaveRequestTest
{
    [Fact]
    public async Task Create_ForGivenApprover_ThrowsInvalidOperationException_WhenApproverIsRequestingEmployee()
    {
        // Arrange
        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee substitutor = EmployeeHelper.CreateEmployee();
        Employee approver = requestingEmployee;

        DateOnly startedAt = new(2024, 2, 15);
        DateOnly endedAt = new(2024, 2, 20);
        Domain.Entities.LeaveType leaveType = await GetDefaultLeaveTypeAsync();

        Mock<ILeaveRequestRuleSet> leaveRequestRuleSetMock = CreateLeaveRequestRuleSetMock();
        SetupLeaveRequestRuleSetMockToReturnResult(leaveRequestRuleSetMock, isRuleFailed: false);

        var expectedExceptionMessage = "Requesting employee cannot be their own approver";

        // Act
        Action result = async () => await LeaveRequest.CreateAsync(
            leaveRequestRuleSetMock.Object,
            requestingEmployee,
            leaveType,
            startedAt,
            endedAt,
            approver,
            substitutor
        );

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Create_ForGivenApprover_ThrowsInvalidOperationException_WhenApproverIsNotSuperiorOfGivenEmployee()
    {
        // Arrange
        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee substitutor = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();

        DateOnly startedAt = new(2024, 2, 15);
        DateOnly endedAt = new(2024, 2, 20);
        Domain.Entities.LeaveType leaveType = await GetDefaultLeaveTypeAsync();

        Mock<ILeaveRequestRuleSet> leaveRequestRuleSetMock = CreateLeaveRequestRuleSetMock();
        SetupLeaveRequestRuleSetMockToReturnResult(leaveRequestRuleSetMock, isRuleFailed: true);

        var expectedExceptionMessage = "Approver must be a superior of requesting employee";

        // Act
        Action result = async () => await LeaveRequest.CreateAsync(
            leaveRequestRuleSetMock.Object,
            requestingEmployee,
            leaveType,
            startedAt,
            endedAt,
            approver,
            substitutor
        );

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Create_ForGivenRequestDateRange_ThrowsInvalidOperationException_WhenStartDateIsGreaterThanEndDate()
    {
        // Arrange
        DateOnly startedAt = new(2024, 2, 24);
        DateOnly endedAt = new(2024, 2, 16);

        Employee requestingEmployee = EmployeeHelper.CreateEmployee();
        Employee substitutor = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();
        Domain.Entities.LeaveType leaveType = await GetDefaultLeaveTypeAsync();

        Mock<ILeaveRequestRuleSet> leaveRequestRuleSetMock = CreateLeaveRequestRuleSetMock();
        SetupLeaveRequestRuleSetMockToReturnResult(leaveRequestRuleSetMock, isRuleFailed: false);

        var expectedExceptionMessage = "Request started date must be fewer than ended date";

        // Act
        Action result = async () => await LeaveRequest.CreateAsync(
            leaveRequestRuleSetMock.Object,
            requestingEmployee,
            leaveType,
            startedAt,
            endedAt,
            approver,
            substitutor
        );

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [MemberData(nameof(GetConflictingLeaveRequestDates))]
    public async Task Create_ForGivenRequestDateRange_ThrowsInvalidOperationException_WhenAnotherRequestWithGivenDateRangeExist(DateOnly startedAt,
                                                                                                                                DateOnly endedAt)
    {
        // Arrange
        Employee requestingEmployee = await EmployeeHelper.CreateEmployeeWithLeaveRequestListAsync(
            approver: null,
            (new DateOnly(2024, 2, 1), new DateOnly(2024, 2, 15)),    
            (new DateOnly(2024, 2, 19), new DateOnly(2024, 2, 22)),    
            (new DateOnly(2024, 3, 1), new DateOnly(2024, 3, 6))    
        );

        Employee substitutor = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();
        Domain.Entities.LeaveType leaveType = await GetDefaultLeaveTypeAsync();

        Mock<ILeaveRequestRuleSet> leaveRequestRuleSetMock = CreateLeaveRequestRuleSetMock();
        SetupLeaveRequestRuleSetMockToReturnResult(leaveRequestRuleSetMock, isRuleFailed: false);

        var expectedExceptionMessage = "Given period for request is already included in another one";

        // Act
        Action result = async () => await LeaveRequest.CreateAsync(
            leaveRequestRuleSetMock.Object,
            requestingEmployee,
            leaveType,
            startedAt,
            endedAt,
            approver,
            substitutor
        );

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Create_ForGivenParams_ReturnsNewInstance()
    {
        // Arrange
        Employee requestingEmployee = await EmployeeHelper.CreateEmployeeWithLeaveRequestListAsync(
            approver: null,
            (new DateOnly(2024, 2, 1), new DateOnly(2024, 2, 15)),
            (new DateOnly(2024, 2, 19), new DateOnly(2024, 2, 22)),
            (new DateOnly(2024, 3, 1), new DateOnly(2024, 3, 6))
        );

        Employee substitutor = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();
        Domain.Entities.LeaveType leaveType = await GetDefaultLeaveTypeAsync();

        string comment = "Comment for leave request";
        string approverComment = "Approver comment for leave request";
        string reasonDescription = "Description for reason";

        DateOnly startedAt = new(2024, 2, 23);
        DateOnly endedAt = new(2024, 2, 27);
        int expectedTotalDays = 5;

        Mock<ILeaveRequestRuleSet> leaveRequestRuleSetMock = CreateLeaveRequestRuleSetMock();
        SetupLeaveRequestRuleSetMockToReturnResult(leaveRequestRuleSetMock, isRuleFailed: false);

        var expectedLeaveRequest = new
        {
            RequestingEmployee = requestingEmployee,
            LeaveType = leaveType,
            StartedAt = startedAt,
            EndedAt = endedAt,
            TotalDays = expectedTotalDays,
            Substitutor = substitutor,
            Comment = comment,
            Approver = approver,
            ApproverComment = approverComment,
            ReasonDescription = reasonDescription,
            Status = RequestStatus.Pending,
        };

        // Act
        LeaveRequest leaveRequest = await LeaveRequest.CreateAsync(
            leaveRequestRuleSetMock.Object,
            requestingEmployee,
            leaveType,
            startedAt,
            endedAt,
            approver,
            substitutor,
            comment,
            approverComment,
            reasonDescription
        );

        // Assert
        leaveRequest
            .Should()
            .BeEquivalentTo(expectedLeaveRequest, options => options
                .Including(lr => lr.RequestingEmployee)
                .Including(lr => lr.LeaveType)
                .Including(lr => lr.StartedAt)
                .Including(lr => lr.EndedAt)
                .Including(lr => lr.TotalDays)
                .Including(lr => lr.Substitutor)
                .Including(lr => lr.Comment)
                .Including(lr => lr.Approver)
                .Including(lr => lr.ApproverComment)
                .Including(lr => lr.ReasonDescription)
                .Including(lr => lr.Status)
            );
    }

    [Fact]
    public async Task Approve_ForGivenRequester_ThrowsArgumentException_WhenRequesterIsNotApprover()
    {
        // Arrange
        Employee substitutor = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();
        Employee invalidApprover = EmployeeHelper.CreateEmployee();

        Employee requestingEmployee = await EmployeeHelper.CreateEmployeeWithLeaveRequestListAsync(
            approver: approver,
            (new DateOnly(2024, 2, 1), new DateOnly(2024, 2, 15))
        );

        Domain.Entities.LeaveType leaveType = await GetDefaultLeaveTypeAsync();
        DateOnly startedAt = new(2024, 2, 24);
        DateOnly endedAt = new(2024, 2, 16);

        Mock<ILeaveRequestRuleSet> leaveRequestRuleSetMock = CreateLeaveRequestRuleSetMock();
        SetupLeaveRequestRuleSetMockToReturnResult(leaveRequestRuleSetMock, isRuleFailed: false);

        LeaveRequest leaveRequest = await LeaveRequest.CreateAsync(
            leaveRequestRuleSetMock.Object,
            requestingEmployee,
            leaveType,
            startedAt,
            endedAt,
            approver,
            substitutor
        );

        var expectedExceptionMessage = "Only approver is allowed to approve the request";

        // Act
        Action result = () => leaveRequest.Approve(invalidApprover);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [MemberData(nameof(GetUpdateStatusActions))]
    public async Task Approve_ThrowsInvalidOperationException_WhenLeaveRequestStatusIsNotPending(Action<LeaveRequest, Employee> updateStatusAction)
    {
        // Arrange
        Employee substitutor = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();

        Employee requestingEmployee = await EmployeeHelper.CreateEmployeeWithLeaveRequestListAsync(
            approver: approver,
            (new DateOnly(2024, 2, 1), new DateOnly(2024, 2, 15))
        );

        Domain.Entities.LeaveType leaveType = await GetDefaultLeaveTypeAsync();
        DateOnly startedAt = new(2024, 2, 24);
        DateOnly endedAt = new(2024, 2, 16);

        Mock<ILeaveRequestRuleSet> leaveRequestRuleSetMock = CreateLeaveRequestRuleSetMock();
        SetupLeaveRequestRuleSetMockToReturnResult(leaveRequestRuleSetMock, isRuleFailed: false);

        LeaveRequest leaveRequest = await LeaveRequest.CreateAsync(
            leaveRequestRuleSetMock.Object,
            requestingEmployee,
            leaveType,
            startedAt,
            endedAt,
            approver,
            substitutor
        );

        updateStatusAction.Invoke(leaveRequest, approver);

        var expectedExceptionMessage = "It is not allowed to approve canceled or rejected request";

        // Act
        Action result = () => leaveRequest.Approve(approver);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Approve_ForGivenApprover_UpdatesStatusOfLeaveRequestProperly()
    {
        // Arrange
        Employee substitutor = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();

        Employee requestingEmployee = await EmployeeHelper.CreateEmployeeWithLeaveRequestListAsync(
            approver: approver,
            (new DateOnly(2024, 2, 1), new DateOnly(2024, 2, 15))
        );

        Domain.Entities.LeaveType leaveType = await GetDefaultLeaveTypeAsync();
        DateOnly startedAt = new(2024, 2, 24);
        DateOnly endedAt = new(2024, 2, 16);

        Mock<ILeaveRequestRuleSet> leaveRequestRuleSetMock = CreateLeaveRequestRuleSetMock();
        SetupLeaveRequestRuleSetMockToReturnResult(leaveRequestRuleSetMock, isRuleFailed: false);

        LeaveRequest leaveRequest = await LeaveRequest.CreateAsync(
            leaveRequestRuleSetMock.Object,
            requestingEmployee,
            leaveType,
            startedAt,
            endedAt,
            approver,
            substitutor
        );

        RequestStatus expectedStatus = RequestStatus.Approved;

        // Act
        leaveRequest.Approve(approver);

        // Assert
        leaveRequest.Status
            .Should()
            .Be(expectedStatus);
    }

    [Fact]
    public async Task Reject_ForGivenRequester_ThrowsArgumentException_WhenRequesterIsNotApprover()
    {
        // Arrange
        Employee substitutor = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();
        Employee invalidApprover = EmployeeHelper.CreateEmployee();

        Employee requestingEmployee = await EmployeeHelper.CreateEmployeeWithLeaveRequestListAsync(
            approver: approver,
            (new DateOnly(2024, 2, 1), new DateOnly(2024, 2, 15))
        );

        Domain.Entities.LeaveType leaveType = await GetDefaultLeaveTypeAsync();
        DateOnly startedAt = new(2024, 2, 24);
        DateOnly endedAt = new(2024, 2, 16);

        Mock<ILeaveRequestRuleSet> leaveRequestRuleSetMock = CreateLeaveRequestRuleSetMock();
        SetupLeaveRequestRuleSetMockToReturnResult(leaveRequestRuleSetMock, isRuleFailed: false);

        LeaveRequest leaveRequest = await LeaveRequest.CreateAsync(
            leaveRequestRuleSetMock.Object,
            requestingEmployee,
            leaveType,
            startedAt,
            endedAt,
            approver,
            substitutor
        );

        var expectedExceptionMessage = "Only approver is allowed to reject the request";

        // Act
        Action result = () => leaveRequest.Reject(invalidApprover);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [MemberData(nameof(GetUpdateStatusActions))]
    public async Task Reject_ThrowsInvalidOperationException_WhenLeaveRequestStatusIsNotPending(Action<LeaveRequest, Employee> updateStatusAction)
    {
        // Arrange
        Employee substitutor = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();

        Employee requestingEmployee = await EmployeeHelper.CreateEmployeeWithLeaveRequestListAsync(
            approver: approver,
            (new DateOnly(2024, 2, 1), new DateOnly(2024, 2, 15))
        );

        Domain.Entities.LeaveType leaveType = await GetDefaultLeaveTypeAsync();
        DateOnly startedAt = new(2024, 2, 24);
        DateOnly endedAt = new(2024, 2, 16);

        Mock<ILeaveRequestRuleSet> leaveRequestRuleSetMock = CreateLeaveRequestRuleSetMock();
        SetupLeaveRequestRuleSetMockToReturnResult(leaveRequestRuleSetMock, isRuleFailed: false);

        LeaveRequest leaveRequest = await LeaveRequest.CreateAsync(
            leaveRequestRuleSetMock.Object,
            requestingEmployee,
            leaveType,
            startedAt,
            endedAt,
            approver,
            substitutor
        );

        updateStatusAction.Invoke(leaveRequest, approver);

        var expectedExceptionMessage = "It is not allowed to reject canceled or approved request";

        // Act
        Action result = () => leaveRequest.Reject(approver);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Reject_ForGivenApprover_UpdatesStatusOfLeaveRequestProperly()
    {
        // Arrange
        Employee substitutor = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();

        Employee requestingEmployee = await EmployeeHelper.CreateEmployeeWithLeaveRequestListAsync(
            approver: approver,
            (new DateOnly(2024, 2, 1), new DateOnly(2024, 2, 15))
        );

        Domain.Entities.LeaveType leaveType = await GetDefaultLeaveTypeAsync();
        DateOnly startedAt = new(2024, 2, 24);
        DateOnly endedAt = new(2024, 2, 16);

        Mock<ILeaveRequestRuleSet> leaveRequestRuleSetMock = CreateLeaveRequestRuleSetMock();
        SetupLeaveRequestRuleSetMockToReturnResult(leaveRequestRuleSetMock, isRuleFailed: false);

        LeaveRequest leaveRequest = await LeaveRequest.CreateAsync(
            leaveRequestRuleSetMock.Object,
            requestingEmployee,
            leaveType,
            startedAt,
            endedAt,
            approver,
            substitutor
        );

        RequestStatus expectedStatus = RequestStatus.Rejected;

        // Act
        leaveRequest.Reject(approver);

        // Assert
        leaveRequest.Status
            .Should()
            .Be(expectedStatus);
    }

    [Fact]
    public async Task Cancel_ForGivenRequester_ThrowsArgumentException_WhenRequesterIsNotRequestingEmployeeOrApprover()
    {
        // Arrange
        Employee substitutor = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();
        Employee randomEmployee = EmployeeHelper.CreateEmployee();

        Employee requestingEmployee = await EmployeeHelper.CreateEmployeeWithLeaveRequestListAsync(
            approver: approver,
            (new DateOnly(2024, 2, 1), new DateOnly(2024, 2, 15))
        );

        Domain.Entities.LeaveType leaveType = await GetDefaultLeaveTypeAsync();
        DateOnly startedAt = new(2024, 2, 24);
        DateOnly endedAt = new(2024, 2, 16);

        Mock<ILeaveRequestRuleSet> leaveRequestRuleSetMock = CreateLeaveRequestRuleSetMock();
        SetupLeaveRequestRuleSetMockToReturnResult(leaveRequestRuleSetMock, isRuleFailed: false);

        LeaveRequest leaveRequest = await LeaveRequest.CreateAsync(
            leaveRequestRuleSetMock.Object,
            requestingEmployee,
            leaveType,
            startedAt,
            endedAt,
            approver,
            substitutor
        );

        var expectedExceptionMessage = "Only approver or requesting-employee are allowed to cancel the request";

        // Act
        Action result = () => leaveRequest.Cancel(randomEmployee);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [MemberData(nameof(GetUpdateStatusActions))]
    public async Task Cancel_ForGivenRequestingEmployeeAsRequester_ThrowsInvalidOperationException_WhenLeaveRequestStatusIsNotPending(Action<LeaveRequest, Employee> updateStatusAction)
    {
        // Arrange
        Employee substitutor = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();

        Employee requestingEmployee = await EmployeeHelper.CreateEmployeeWithLeaveRequestListAsync(
            approver: approver,
            (new DateOnly(2024, 2, 1), new DateOnly(2024, 2, 15))
        );

        Domain.Entities.LeaveType leaveType = await GetDefaultLeaveTypeAsync();
        DateOnly startedAt = new(2024, 2, 24);
        DateOnly endedAt = new(2024, 2, 16);

        Mock<ILeaveRequestRuleSet> leaveRequestRuleSetMock = CreateLeaveRequestRuleSetMock();
        SetupLeaveRequestRuleSetMockToReturnResult(leaveRequestRuleSetMock, isRuleFailed: false);

        LeaveRequest leaveRequest = await LeaveRequest.CreateAsync(
            leaveRequestRuleSetMock.Object,
            requestingEmployee,
            leaveType,
            startedAt,
            endedAt,
            approver,
            substitutor
        );

        updateStatusAction.Invoke(leaveRequest, approver);

        var expectedExceptionMessage = "Only approver is allowed to cancel request after rejecting or approving";

        // Act
        Action result = () => leaveRequest.Cancel(requestingEmployee);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Cancel_ForGivenRequestingEmployeeAsRequester_UpdatesStatusOfLeaveRequestProperly()
    {
        // Arrange
        Employee substitutor = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();

        Employee requestingEmployee = await EmployeeHelper.CreateEmployeeWithLeaveRequestListAsync(
            approver: approver,
            (new DateOnly(2024, 2, 1), new DateOnly(2024, 2, 15))
        );

        Domain.Entities.LeaveType leaveType = await GetDefaultLeaveTypeAsync();
        DateOnly startedAt = new(2024, 2, 24);
        DateOnly endedAt = new(2024, 2, 16);

        Mock<ILeaveRequestRuleSet> leaveRequestRuleSetMock = CreateLeaveRequestRuleSetMock();
        SetupLeaveRequestRuleSetMockToReturnResult(leaveRequestRuleSetMock, isRuleFailed: false);

        LeaveRequest leaveRequest = await LeaveRequest.CreateAsync(
            leaveRequestRuleSetMock.Object,
            requestingEmployee,
            leaveType,
            startedAt,
            endedAt,
            approver,
            substitutor
        );

        RequestStatus expectedStatus = RequestStatus.Canceled;

        // Act
        leaveRequest.Cancel(requestingEmployee);

        // Assert
        leaveRequest.Status
            .Should()
            .Be(expectedStatus);
    }

    [Theory]
    [MemberData(nameof(GetUpdateStatusActions))]
    public async Task Cancel_ForGivenApproverAsRequester_UpdatesStatusOfLeaveRequestProperly(Action<LeaveRequest, Employee> updateStatusAction)
    {
        // Arrange
        Employee substitutor = EmployeeHelper.CreateEmployee();
        Employee approver = EmployeeHelper.CreateEmployee();

        Employee requestingEmployee = await EmployeeHelper.CreateEmployeeWithLeaveRequestListAsync(
            approver: approver,
            (new DateOnly(2024, 2, 1), new DateOnly(2024, 2, 15))
        );

        Domain.Entities.LeaveType leaveType = await GetDefaultLeaveTypeAsync();
        DateOnly startedAt = new(2024, 2, 24);
        DateOnly endedAt = new(2024, 2, 16);

        Mock<ILeaveRequestRuleSet> leaveRequestRuleSetMock = CreateLeaveRequestRuleSetMock();
        SetupLeaveRequestRuleSetMockToReturnResult(leaveRequestRuleSetMock, isRuleFailed: false);

        LeaveRequest leaveRequest = await LeaveRequest.CreateAsync(
            leaveRequestRuleSetMock.Object,
            requestingEmployee,
            leaveType,
            startedAt,
            endedAt,
            approver,
            substitutor
        );

        updateStatusAction.Invoke(leaveRequest, approver);

        RequestStatus expectedStatus = RequestStatus.Canceled;

        // Act
        leaveRequest.Cancel(approver);

        // Assert
        leaveRequest.Status
            .Should()
            .Be(expectedStatus);
    }

    public static IEnumerable<object[]> GetUpdateStatusActions()
        => [
            [(LeaveRequest leaveRequest, Employee approver) => leaveRequest.Approve(approver)],
            [(LeaveRequest leaveRequest, Employee approver) => leaveRequest.Reject(approver)],
            [(LeaveRequest leaveRequest, Employee approver) => leaveRequest.Cancel(approver)]
        ];

    public static IEnumerable<object[]> GetConflictingLeaveRequestDates()
        => [
            [new DateOnly(2024, 2, 10), new DateOnly(2024, 2, 20)],
            [new DateOnly(2024, 2, 5), new DateOnly(2024, 2, 10)],
            [new DateOnly(2024, 1, 28), new DateOnly(2024, 2, 2)],
            [new DateOnly(2024, 2, 12), new DateOnly(2024, 2, 18)],
            [new DateOnly(2024, 2, 19), new DateOnly(2024, 2, 21)],
            [new DateOnly(2024, 3, 1), new DateOnly(2024, 3, 6)],
            [new DateOnly(2024, 3, 5), new DateOnly(2024, 3, 10)],
            [new DateOnly(2024, 2, 14), new DateOnly(2024, 2, 20)],
            [new DateOnly(2024, 2, 1), new DateOnly(2024, 2, 15)],
            [new DateOnly(2024, 2, 3), new DateOnly(2024, 2, 13)],
        ];

    #region Test_Factory_Methods

    private static Mock<ILeaveRequestRuleSet> CreateLeaveRequestRuleSetMock() => new();

    private static void SetupLeaveRequestRuleSetMockToReturnResult(Mock<ILeaveRequestRuleSet> leaveRequestRuleSetMock,
                                                                   bool isRuleFailed)
        => leaveRequestRuleSetMock
            .Setup(rule => rule.IsRequestApproverSuperiorOfEmployeeAsync(
                It.IsAny<Employee>(), It.IsAny<Employee>(), CancellationToken.None)
            )
            .ReturnsAsync(!isRuleFailed);

    private static async Task<Domain.Entities.LeaveType> GetDefaultLeaveTypeAsync() => await LeaveTypeHelper.CreateLeaveTypeAsync("Vacation leave");

    #endregion
}
