using HRLeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HRLeaveManagement.Identity.Tests.Mocks;

public static class SignInManagerMock
{
    public static Mock<SignInManager<ApplicationUser>> Create()
        => new(
            UserManagerMock.Create().Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<ApplicationUser>>().Object
        );

    public static Mock<SignInManager<ApplicationUser>> Create(UserManager<ApplicationUser> userManager)
        => new(
            userManager,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<ApplicationUser>>().Object
        );

    public static void SetupToReturnSignInResult(Mock<SignInManager<ApplicationUser>> signInManagerMock,
                                                 SignInResult signInResult)
        => signInManagerMock
            .Setup(sim => sim.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), false))
            .ReturnsAsync(signInResult);
}
