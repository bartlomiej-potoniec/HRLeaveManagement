namespace HRLeaveManagement.Domain.Tests.Entities;

public class LeaveTypeTest
{
    [Fact]
    public void Create_ForGivenParams_CreatesNewInstance()
    {
        // Arrange
        string name = "Employee Leave";

        // Act
        var leaveType = LeaveType.Create(name);

        // Assert
        leaveType
            .Should()
            .BeOfType<LeaveType>();
    }

    [Fact]
    public void Update_ForGivenParams_UpdatesPropertiesOfExistingInstance()
    {
        // Arrange
        string name = "Employee Leave";
        string description = "Leave Description";
        decimal paidFraction = 0.5M;

        var leaveType = LeaveType.Create("leave");
        var expectedLeaveType = LeaveType.Create(name, description, paidFraction);

        // Act
        LeaveType.Update(leaveType, name, description, paidFraction);

        // Assert
        leaveType
            .Should()
            .BeEquivalentTo(expectedLeaveType, options => options
                .Including(lt => lt.Name)
                .Including(lt => lt.Description)
                .Including(lt => lt.PaidFraction)
            );
    }
}
