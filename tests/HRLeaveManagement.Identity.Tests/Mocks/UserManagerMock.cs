using HRLeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HRLeaveManagement.Identity.Tests.Mocks;

public class UserManagerMock
{
    public static Mock<UserManager<ApplicationUser>> GetUserManagerMock()
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
    

    public static Mock<UserManager<ApplicationUser>> GetUserManagerMock(IQueryable<ApplicationUser> users)
    {
        var userManagerMock = GetUserManagerMock();
        userManagerMock
            .Setup(u => u.Users)
            .Returns(users);

        return userManagerMock;
    }
}
