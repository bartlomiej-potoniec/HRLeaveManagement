using HRLeaveManagement.Domain.Tests.Helpers;
using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class WorkRequestTest
{
    private readonly WorkRequest _workRequest;

    public WorkRequestTest()
        => _workRequest = new WorkRequestTestHelper();

    [Fact]
    public void InitializeBase_ForGivenParams_ReturnsNewInstance()
    {
        // Arrange
        Guid requestingEmployeeId = Guid.NewGuid();
        DateOnly startedAt = new(2024, 11, 11);
        DateOnly endedAt = new(2024, 11, 13);
        Guid approverId = Guid.NewGuid();

        // Act
        ((WorkRequestTestHelper)_workRequest)
            .InitializeBase(requestingEmployeeId, startedAt, endedAt, approverId);

        // Assert
        _workRequest
            .Should()
            .BeAssignableTo<WorkRequest>();
    }

    [Theory]
    [MemberData(nameof(GetData))]
    public void UpdateStatus_ForGivenEntity_ChangesAppropriateStatus(Action<WorkRequest> workRequest,
                                                                     RequestStatus requestStatus)
    {
        // Act
        workRequest.Invoke(_workRequest);

        // Assert
        _workRequest
            .Status
            .Should()
            .Be(requestStatus);
    }

    public static IEnumerable<object[]> GetData()
        => [
            [(WorkRequest workRequest) => WorkRequest.Approve(workRequest), RequestStatus.Approved],
            [(WorkRequest workRequest) => WorkRequest.Reject(workRequest), RequestStatus.Rejected],
            [(WorkRequest workRequest) => WorkRequest.Cancel(workRequest), RequestStatus.Canceled],
        ];
}
