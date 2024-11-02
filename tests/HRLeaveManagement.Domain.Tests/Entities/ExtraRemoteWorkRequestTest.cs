namespace HRLeaveManagement.Domain.Tests.Entities;

public class ExtraRemoteWorkRequestTest
{
    [Fact]
    public void Create_ForGivenParams_ReturnsNewInstance()
    {
        // Arrange
        Guid requestingEmployeeId = Guid.NewGuid();
        DateOnly startedAt = new(2024, 11, 11);
        DateOnly endedAt = new(2024, 11, 13);
        Guid approverId = Guid.NewGuid();

        // Act
        var delegationRequest = ExtraRemoteWorkRequest.Create(
            requestingEmployeeId,
            startedAt,
            endedAt,
            approverId
        );

        // Assert
        delegationRequest
            .Should()
            .BeOfType<ExtraRemoteWorkRequest>();
    }
}
