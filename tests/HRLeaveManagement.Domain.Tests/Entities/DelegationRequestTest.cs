namespace HRLeaveManagement.Domain.Tests.Entities;

public class DelegationRequestTest
{
    [Fact]
    public void Create_ForGivenParams_ReturnsNewInstance()
    {
        // Arrange
        Guid requestingEmployeeId = Guid.NewGuid();
        DateOnly startedAt = new(2024, 11, 11);
        DateOnly endedAt = new(2024, 11, 13);
        Guid approverId = Guid.NewGuid();
        Guid substitutorId = Guid.NewGuid();
        string destinationCountry = "Germany";
        string meansOfTransport = "By car";
        decimal cashAdvance = 150.5M;

        // Act
        var delegationRequest = DelegationRequest.Create(
            requestingEmployeeId,
            startedAt,
            endedAt,
            approverId,
            substitutorId,
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
