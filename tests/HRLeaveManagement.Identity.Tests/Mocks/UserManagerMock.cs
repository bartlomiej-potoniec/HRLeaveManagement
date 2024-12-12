using HRLeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace HRLeaveManagement.Identity.Tests.Mocks;

public class UserManagerMock
{
    public static Mock<UserManager<ApplicationUser>> Create()
        => new(
            new Mock<IUserStore<ApplicationUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<ApplicationUser>>().Object,
            Array.Empty<IUserValidator<ApplicationUser>>(),
            Array.Empty<IPasswordValidator<ApplicationUser>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object
        );
    

    public static Mock<UserManager<ApplicationUser>> Create(IQueryable<ApplicationUser> users)
    {
        var userManagerMock = Create();
        userManagerMock
            .Setup(u => u.Users)
            .Returns(users);

        return userManagerMock;
    }

    public static void SetupToFindUserByName(Mock<UserManager<ApplicationUser>> userManagerMock,
                                             ApplicationUser? user)
        => userManagerMock
            .Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

    public static void SetupToFindUserById(Mock<UserManager<ApplicationUser>> userManagerMock,
                                           ApplicationUser? user)
        => userManagerMock
            .Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

    public static void SetupCreateToReturnIdentityResult(Mock<UserManager<ApplicationUser>> userManagerMock,
                                                         IdentityResult identityResult)
        => userManagerMock
            .Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(identityResult);

    public static void SetupAddToRoleToReturnIdentityResult(Mock<UserManager<ApplicationUser>> userManagerMock,
                                                            IdentityResult identityResult)
        => userManagerMock
            .Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(identityResult);

    public static void SetupGetUserToFindByApplicationUser(Mock<UserManager<ApplicationUser>> userManagerMock,
                                                           ApplicationUser? user)
        => userManagerMock
            .Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(() => user);

    public static void SetupChangePasswordToReturnIdentityResult(Mock<UserManager<ApplicationUser>> userManagerMock,
                                                               IdentityResult identityResult)
        => userManagerMock
            .Setup(um => um.ChangePasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(identityResult);
}
