using HRLeaveManagement.Domain.RuleContracts;
using System.Xml.Linq;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class LeaveTypeTest
{
    [Fact]
    public void Create_ForGivenName_ThrowsInvalidOperationException_WhenLeaveTypeWithGivenNameAlreadyExist()
    {
        // Arrange
        string name = "Vacation leave";

        Mock<ILeaveTypeRuleSet> leaveTypeRuleSetMock = CreateLeaveTypeRuleSetMock();
        SetupLeaveTypeRuleSetMockToReturnResult(leaveTypeRuleSetMock, isRuleFailed: true);

        var expectedExceptionMessage = "Leave type with name 'Vacation leave' already exists";

        // Act
        Action result = async () => await LeaveType.CreateAsync(leaveTypeRuleSetMock.Object, name);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void Create_ForGivenPaidFraction_ThrowsArgumentException_WhenGivenPaidFractionIsFewerThanZero()
    {
        // Arrange
        string name = "Vacation leave";
        decimal paidFraction = -2.0M;

        Mock<ILeaveTypeRuleSet> leaveTypeRuleSetMock = CreateLeaveTypeRuleSetMock();
        SetupLeaveTypeRuleSetMockToReturnResult(leaveTypeRuleSetMock, isRuleFailed: false);

        var expectedExceptionMessage = "Paid fraction of leave type must be greater than 0";

        // Act
        Action result = async () => await LeaveType.CreateAsync(leaveTypeRuleSetMock.Object, name, paidFraction);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Create_ForGivenParams_CreatesNewInstance()
    {
        // Arrange
        string name = "Vacation leave";
        decimal paidFraction = -2.0M;
        string description = "Description for Vacation leave";

        Mock<ILeaveTypeRuleSet> leaveTypeRuleSetMock = CreateLeaveTypeRuleSetMock();
        SetupLeaveTypeRuleSetMockToReturnResult(leaveTypeRuleSetMock, isRuleFailed: false);

        var expectedLeaveType = new
        {
            Name = name,
            Description = description,
            PaidFraction = paidFraction,
        };

        // Act
        var leaveType = await LeaveType.CreateAsync(leaveTypeRuleSetMock.Object, name, paidFraction, description);

        // Assert
        leaveType
            .Should()
            .BeEquivalentTo(expectedLeaveType, options => options
                .Including(lt => lt.Name)
                .Including(lt => lt.Description)
                .Including(lt => lt.PaidFraction)
            );
    }

    [Fact]
    public async Task Update_ForGivenParams_UpdatesPropertiesOfExistingInstance()
    {
        // Arrange
        string nameToUpdate = "Vacation leave";
        decimal paidFractionToUpdate = -2.0M;
        string descriptionToUpdate = "Description for Vacation leave";

        Mock<ILeaveTypeRuleSet> leaveTypeRuleSetMock = CreateLeaveTypeRuleSetMock();
        SetupLeaveTypeRuleSetMockToReturnResult(leaveTypeRuleSetMock, isRuleFailed: false);

        LeaveType leaveType = await LeaveType.CreateAsync(leaveTypeRuleSetMock.Object, "Some leave");

        var expectedLeaveType = new
        {
            Name = nameToUpdate,
            Description = descriptionToUpdate,
            PaidFraction = paidFractionToUpdate,
        };

        // Act
        await leaveType.UpdateAsync(leaveTypeRuleSetMock.Object, nameToUpdate, paidFractionToUpdate, descriptionToUpdate);

        // Assert
        leaveType
            .Should()
            .BeEquivalentTo(expectedLeaveType, options => options
                .Including(lt => lt.Name)
                .Including(lt => lt.Description)
                .Including(lt => lt.PaidFraction)
            );
    }

    #region Test_Factory_Methods

    private static Mock<ILeaveTypeRuleSet> CreateLeaveTypeRuleSetMock() => new();

    private static void SetupLeaveTypeRuleSetMockToReturnResult(Mock<ILeaveTypeRuleSet> leaveTypeRuleSetMock,
                                                                bool isRuleFailed)
        => leaveTypeRuleSetMock
            .Setup(rule => rule.IsNameUniqueAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(!isRuleFailed);

    #endregion
}
