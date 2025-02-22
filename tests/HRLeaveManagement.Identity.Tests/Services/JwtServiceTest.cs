using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Identity.Models;
using HRLeaveManagement.Identity.Options;
using HRLeaveManagement.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace HRLeaveManagement.Identity.Tests.Services;

public class JwtServiceTest
{
    [Fact]
    public async Task GenerateJwtToken_ThrowsNotFoundException_WhenUserNotFound()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var userName = "jkowals65";

        userManagerMock
            .Setup(um => um.FindByNameAsync(userName))
            .ReturnsAsync(() => null);

        var jwtService = CreateJwtService(userManagerMock);
        var expectedExceptionMessage = "User with username: jkowals65 not found";

        // Act
        Func<Task> result = () => jwtService.GenerateJwtTokenAsync(userName);

        // Assert
        await result
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task GenerateJwtToken_GeneratesNewJwtToken()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var userName = "jkowals65";
        var user = ApplicationUserMock.Create();

        userManagerMock
            .Setup(um => um.FindByNameAsync(userName))
            .ReturnsAsync(user);

        userManagerMock
            .Setup(um => um.GetClaimsAsync(user))
            .ReturnsAsync([]);

        userManagerMock
            .Setup(um => um.GetRolesAsync(user))
            .ReturnsAsync([]);

        var jwtService = CreateJwtService(userManagerMock);

        // Act
        var result = await jwtService.GenerateJwtTokenAsync(userName);

        // Assert
        result
            .Should()
            .BeOfType<string>();
    }

    #region Test_Factory_Methods

    private static Mock<UserManager<ApplicationUser>> CreateUserManagerMock() => new();
    private static OptionsWrapper<JwtOptions> CreateOptionsWrapper(JwtOptions options) => new(options);

    private static JwtService CreateJwtService(Mock<UserManager<ApplicationUser>>? userManagerMock = null)
    {
        userManagerMock ??= CreateUserManagerMock();

        var jwtOptions = CreateJwtOptions();
        var options = CreateOptionsWrapper(jwtOptions);

        return new(userManagerMock.Object, options);
    }

    private static JwtOptions CreateJwtOptions()
        => new() 
        {
            Key = "super_secure_security_hmac_sha_256_key",
            Issuer = "HRLeavemanagement",
            Audience = "HRLeavemanagementUser",
            DurationInMinutes = 60
        };

    #endregion
}
