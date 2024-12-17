using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.DTOs.Auth;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Identity.DbContexts;
using HRLeaveManagement.Identity.Models;
using HRLeaveManagement.Identity.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HRLeaveManagement.Identity.Tests.Services;

public class AuthServiceTest
{
    [Fact]
    public async Task Login_ThrowsNotFoundException_WhenNoUserInUserManagerFound()
    {
        // Arrange
        var signInManagerMock = SignInManagerMock.Create();
        var authRequest = CreateAuthRequest();
        var authService = CreateAuthService(signInManagerMock);

        var expectedExceptionMessage = "User with username: jkowals95 not found";

        // Act
        Func<Task> login = () => authService.Login(authRequest);

        // Assert
        await login
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Login_ForAuthCredentials_ThrowsBadRequestException_WhenIncorrectPassword()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var signInManagerMock = SignInManagerMock.Create(userManagerMock.Object);

        var user = CreateAplicationUser();
        var authRequest = CreateAuthRequest();

        UserManagerMock.SetupToFindUserByName(userManagerMock, user);
        SignInManagerMock.SetupToReturnSignInResult(signInManagerMock, SignInResult.Failed);

        var authService = CreateAuthService(signInManagerMock);

        var expectedExceptionMessage = "Credentials for 'jkowals95' are not valid";

        // Act
        Func<Task> login = () => authService.Login(authRequest);

        // Assert
        await login
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Login_ForAuthCredentials_GeneratesAuthResponse()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var signInManagerMock = SignInManagerMock.Create(userManagerMock.Object);
        var jwtService = CreateJwtServiceMock();

        var user = CreateAplicationUser();
        var authRequest = CreateAuthRequest();
        var token = "test_jwt_security_token";

        UserManagerMock.SetupToFindUserByName(userManagerMock, user);
        SignInManagerMock.SetupToReturnSignInResult(signInManagerMock, SignInResult.Success);

        jwtService
            .Setup(jwt => jwt.GenerateJwtToken(authRequest.UserName))
            .ReturnsAsync(token);

        var authService = CreateAuthService(signInManagerMock, jwtServiceMock: jwtService);

        var expectedAuthResponse = new AuthResponse(user.Id, user.UserName!, user.Email!, token);

        // Act
        var authResponse = await authService.Login(authRequest);

        // Assert
        authResponse
            .Should()
            .BeEquivalentTo(expectedAuthResponse);
    }

    [Fact]
    public async Task Register_ForRegistrationCredentials_ThrowsBadRequestException_WhenCreatingUserFailed()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var signInManagerMock = SignInManagerMock.Create(userManagerMock.Object);
        var credentialServiceMock = CreateCredentialServiceMock();

        var registrationRequest = CreateRegistrationRequest();

        SetupCredentialServiceMockToReturnLogin(credentialServiceMock, "jkowals95");
        SetupCredentialServiceMockToReturnPassword(credentialServiceMock, "1$g&J*34");

        UserManagerMock.SetupCreateToReturnIdentityResult(userManagerMock, IdentityResult.Failed());

        var authService = CreateAuthService(signInManagerMock, credentialServiceMock: credentialServiceMock);

        var expectedExceptionMessage = "Cannot create a new user for given credentials";

        // Act
        Func<Task> result = () => authService.Register(registrationRequest);

        // Assert
        await result
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Register_ForRegistrationCredentials_ThrowsBadRequestException_WhenAddingToRoleFailed()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var signInManagerMock = SignInManagerMock.Create(userManagerMock.Object);
        var credentialServiceMock = CreateCredentialServiceMock();

        var registrationRequest = CreateRegistrationRequest();

        SetupCredentialServiceMockToReturnLogin(credentialServiceMock, "jkowals95");
        SetupCredentialServiceMockToReturnPassword(credentialServiceMock, "1$g&J*34");

        UserManagerMock.SetupCreateToReturnIdentityResult(userManagerMock, IdentityResult.Success);
        UserManagerMock.SetupAddToRoleToReturnIdentityResult(userManagerMock, IdentityResult.Failed());

        var authService = CreateAuthService(signInManagerMock, credentialServiceMock: credentialServiceMock);

        var expectedExceptionMessage = "Cannot add a new user to role 'Employee'";

        // Act
        Func<Task> result = () => authService.Register(registrationRequest);

        // Assert
        await result
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Register_ForRegistrationCredentials_GeneratesRegistrationResponse()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var signInManagerMock = SignInManagerMock.Create(userManagerMock.Object);
        var credentialServiceMock = CreateCredentialServiceMock();
        var emailServiceMock = CreateEmailServiceMock();

        var registrationRequest = CreateRegistrationRequest();
        var token = "email_confirmation_token";
        var confirmationLink = "email_confirmation_link";

        SetupCredentialServiceMockToReturnLogin(credentialServiceMock, "jkowals95");
        SetupCredentialServiceMockToReturnPassword(credentialServiceMock, "1$g&J*34");

        UserManagerMock.SetupCreateToReturnIdentityResult(userManagerMock, IdentityResult.Success);
        UserManagerMock.SetupAddToRoleToReturnIdentityResult(userManagerMock, IdentityResult.Success);

        userManagerMock
            .Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(token);

        emailServiceMock
            .Setup(es => es.GenerateEmailConfirmationLink(It.IsAny<string>(), token))
            .Returns(confirmationLink);

        var authService = CreateAuthService(signInManagerMock, credentialServiceMock, emailServiceMock);

        // Act
        var registrationResponse = await authService.Register(registrationRequest);

        // Assert
        registrationResponse
            .Should()
            .BeOfType<RegistrationResponse>();
    }

    [Theory]
    [InlineData("user_id", null)]
    [InlineData(null, "token")]
    [InlineData(null, null)]
    public async Task ConfirmEmail_ForNullableUserIdOrToken_ThrowsBadRequestException(string? userId,
                                                                                      string? token)
    {
        // Arrange
        var signInManagerMock = SignInManagerMock.Create();
        var authService = CreateAuthService(signInManagerMock);
        var expectedExceptionMessage = "Invalid user ID or token";

        // Act
        Func<Task> result = () => authService.ConfirmEmail(userId, token);

        // Assert
        await result
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task ConfirmEmail_ThrowsNotFoundException_WhenUserNotFound()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var signInManagerMock = SignInManagerMock.Create(userManagerMock.Object);

        string userId = "user_id";
        string token = "token";

        UserManagerMock.SetupToFindUserById(userManagerMock, user: null);

        var authService = CreateAuthService(signInManagerMock);
        var expectedExceptionMessage = "No user with ID: user_id found";

        // Act
        Func<Task> result = () => authService.ConfirmEmail(userId, token);

        // Assert
        await result
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task ConfirmEmail_ThrowsBadRequestException_WhenEmailAlreadyConfirmed()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var signInManagerMock = SignInManagerMock.Create(userManagerMock.Object);

        string userId = "user_id";
        string token = "token";
        var user = CreateAplicationUser();

        UserManagerMock.SetupToFindUserById(userManagerMock, user: user);

        userManagerMock
            .Setup(um => um.IsEmailConfirmedAsync(user))
            .ReturnsAsync(true);

        var authService = CreateAuthService(signInManagerMock);
        var expectedExceptionMessage = "Email for jkowalski95@company.com is already confirmed";

        // Act
        Func<Task> result = () => authService.ConfirmEmail(userId, token);

        // Assert
        await result
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task ConfirmEmail_ThrowsBadRequestException_WhenEmailConfirmationFailed()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var signInManagerMock = SignInManagerMock.Create(userManagerMock.Object);

        string userId = "user_id";
        string token = "token";
        var user = CreateAplicationUser();

        UserManagerMock.SetupToFindUserById(userManagerMock, user: user);

        userManagerMock
            .Setup(um => um.IsEmailConfirmedAsync(user))
            .ReturnsAsync(false);

        userManagerMock
            .Setup(um => um.ConfirmEmailAsync(user, token))
            .ReturnsAsync(IdentityResult.Failed());

        var authService = CreateAuthService(signInManagerMock);
        var expectedExceptionMessage = "Failed to confirm email for jkowalski95@company.com";

        // Act
        Func<Task> result = () => authService.ConfirmEmail(userId, token);

        // Assert
        await result
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task ChangePassword_ThrowsNotFoundException_WhenUserContextNotFound()
    {
        // Arrange
        var newPassword = "P@ssword2";
        var passwordRequest = CreatePasswordRequest("P@ssword1", newPassword);

        var signInManagerMock = SignInManagerMock.Create();
        var userServiceMock = CreateUserServiceMock();

        SetupUserServiceMockToReturnUser(userServiceMock, null);

        var authService = CreateAuthService(signInManagerMock, userServiceMock: userServiceMock);

        var expectedExceptionMessage = "No user found in current context";

        // Act
        Func<Task> result = () => authService.ChangePassword(passwordRequest);

        // Assert
        await result
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task ChangePassword_ThrowsNotFoundException_WhenUserNotFound()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var signInManagerMock = SignInManagerMock.Create(userManagerMock.Object);
        var userServiceMock = CreateUserServiceMock();

        var newPassword = "P@ssword2";
        var passwordRequest = CreatePasswordRequest("P@ssword1", newPassword);

        SetupUserServiceMockToReturnUser(userServiceMock, new ClaimsPrincipal());
        UserManagerMock.SetupGetUserToFindByApplicationUser(userManagerMock, null);

        var authService = CreateAuthService(signInManagerMock, userServiceMock: userServiceMock);

        var expectedExceptionMessage = "No user found";

        // Act
        Func<Task> result = () => authService.ChangePassword(passwordRequest);

        // Assert
        await result
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task ChangePassword_BadRequestException_WhenChangingPasswordFailed()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var signInManagerMock = SignInManagerMock.Create(userManagerMock.Object);
        var userServiceMock = CreateUserServiceMock();

        var currentPassword = "P@ssword1";
        var newPassword = "P@ssword2";
        var passwordRequest = CreatePasswordRequest(currentPassword, newPassword);
        var user = CreateAplicationUser();

        SetupUserServiceMockToReturnUser(userServiceMock, new ClaimsPrincipal());
        UserManagerMock.SetupGetUserToFindByApplicationUser(userManagerMock, user);

        userManagerMock
            .Setup(um => um.ChangePasswordAsync(It.IsAny<ApplicationUser>(), currentPassword, newPassword))
            .ReturnsAsync(IdentityResult.Failed());

        var authService = CreateAuthService(signInManagerMock, userServiceMock: userServiceMock);

        var expectedExceptionMessage = "Failed to change password for user jkowalski95@company.com";

        // Act
        Func<Task> result = () => authService.ChangePassword(passwordRequest);

        // Assert
        await result
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage(expectedExceptionMessage);
    }

    #region Test_Factory_Methods

    private static Mock<ICredentialService> CreateCredentialServiceMock() => new();
    private static Mock<IJwtService> CreateJwtServiceMock() => new();
    private static Mock<IEmailService> CreateEmailServiceMock() => new();
    private static Mock<IUserService> CreateUserServiceMock() => new();

    private static void SetupCredentialServiceMockToReturnLogin(Mock<ICredentialService> credentialServiceMock,
                                                                string login)
        => credentialServiceMock
            .Setup(cs => cs.GenerateUserLogin(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(login);

    private static void SetupCredentialServiceMockToReturnPassword(Mock<ICredentialService> credentialServiceMock,
                                                                   string password)
        => credentialServiceMock
            .Setup(cs => cs.GenerateUserPassword())
            .Returns(password);

    private static void SetupUserServiceMockToReturnUser(Mock<IUserService> userServiceMock,
                                                         ClaimsPrincipal? user)
        => userServiceMock
            .Setup(us => us.User)
            .Returns(() => user);

    private static AuthRequest CreateAuthRequest() => new("jkowals95", "P@ssword1");

    private static RegistrationRequest CreateRegistrationRequest()
       => new("Jan", "Kowalski", "jkowalski95@company.com", new(1995, 4, 12), "12345678911", "566889111", ["Employee"]);

    private static PasswordRequest CreatePasswordRequest(string currentPassword, string newPassword)
        => new(currentPassword, newPassword);

    private static AuthService CreateAuthService(Mock<SignInManager<ApplicationUser>> signInManagerMock,
                                                 Mock<ICredentialService>? credentialServiceMock = null,
                                                 Mock<IEmailService>? emailServiceMock = null,
                                                 Mock<IJwtService>? jwtServiceMock = null,
                                                 Mock<IUserService>? userServiceMock = null)
    {
        var serviceProviderMock = new Mock<IServiceProvider>();
        var identityResultMock = new Mock<IIdentityResult>();
        var loggerMock = new Mock<IAppLogger<AuthService>>();

        credentialServiceMock ??= CreateCredentialServiceMock();
        emailServiceMock ??= CreateEmailServiceMock();
        jwtServiceMock ??= CreateJwtServiceMock();
        userServiceMock ??= CreateUserServiceMock();

        return new(
            signInManagerMock.Object,
            serviceProviderMock.Object,
            credentialServiceMock.Object,
            jwtServiceMock.Object,
            emailServiceMock.Object,
            userServiceMock.Object,
            identityResultMock.Object,
            loggerMock.Object
        );
    }

    private static ApplicationUser CreateAplicationUser()
        => new()
        {
            UserName = "jkowals95",
            Email = "jkowalski95@company.com",
            PasswordHash = "password_hash_for_P@ssword1",
            FirstName = "Jan",
            LastName = "Kowalski",
            DateOfBirth = new(1995, 4, 12),
            EmailConfirmed = false
        };

    #endregion
}
