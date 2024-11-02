using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class LeaveRequestTest
{
    [Fact]
    public void Create_ForGivenParams_ReturnsNewInstance()
    {
        // Act
        var leaveRequest = CreateWithDefaultValues();

        // Assert
        leaveRequest
            .Should()
            .BeOfType<LeaveRequest>();
    }

    [Theory]
    [MemberData(nameof(GetData))]
    public void UpdateStatus_ForGivenEntity_SetsAppropriateStatusValue(Action<LeaveRequest> updateStatus,
                                                                       RequestStatus expectedRequestStatus)
    {
        // Arrange
        var leaveRequest = CreateWithDefaultValues();

        // Act
        updateStatus.Invoke(leaveRequest);

        // Assert
        leaveRequest
            .Status
            .Should()
            .Be(expectedRequestStatus);
    }

    public static IEnumerable<object[]> GetData()
        => [
            [(LeaveRequest entity) => LeaveRequest.Approve(entity), RequestStatus.Approved],
            [(LeaveRequest entity) => LeaveRequest.Reject(entity), RequestStatus.Rejected],
            [(LeaveRequest entity) => LeaveRequest.Cancel(entity), RequestStatus.Canceled],
        ];


    private static LeaveRequest CreateWithDefaultValues()
        =>
            LeaveRequest.Create(
                requestingEmployeeId: Guid.NewGuid(),
                leaveTypeId: 1,
                startedAt: new DateOnly(2024, 6, 6),
                endedAt: new DateOnly(2024, 6, 20),
                substitutorId: Guid.NewGuid(),
                approverId: Guid.NewGuid()
            );
}
