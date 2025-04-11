using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.DTOs.Auth;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Identity.DbContexts;
using HRLeaveManagement.Identity.Models;
using HRLeaveManagement.Identity.Services;
using HRLeaveManagement.Identity.Tests.Fakes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
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
        Func<Task> login = async () => await authService.LoginAsync(authRequest);

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

        UserManagerMock.SetupToFindUserByName(userManagerMock, result: user);
        SignInManagerMock.SetupToReturnSignInResult(signInManagerMock, SignInResult.Failed);

        var authService = CreateAuthService(signInManagerMock);
        var expectedExceptionMessage = "Credentials for 'jkowals95' are not valid";

        // Act
        Func<Task> login = async () => await authService.LoginAsync(authRequest);

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

        UserManagerMock.SetupToFindUserByName(userManagerMock, result: user);
        SignInManagerMock.SetupToReturnSignInResult(signInManagerMock, SignInResult.Success);

        jwtService
            .Setup(jwt => jwt.GenerateJwtTokenAsync(authRequest.UserName, CancellationToken.None))
            .ReturnsAsync(token);

        var authService = CreateAuthService(signInManagerMock, jwtServiceMock: jwtService);

        var expectedAuthResponse = new AuthResponse(user.Id, user.UserName!, user.Email!, token);

        // Act
        var authResponse = await authService.LoginAsync(authRequest);

        // Assert
        authResponse
            .Should()
            .BeEquivalentTo(expectedAuthResponse);
    }

    [Fact]
    public async Task Register_ThrowsOperationCanceledException_WhenBeginTransactionFailed()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var signInManagerMock = SignInManagerMock.Create(userManagerMock.Object);
        var credentialServiceMock = CreateCredentialServiceMock();
        var serviceProviderMock = CreateServiceProviderMock();
        var fakeDbContext = CreateFakeApplicationIdentityDbContext();

        var registrationRequest = CreateRegistrationRequest();

        SetupCredentialServiceMockToReturnLogin(credentialServiceMock, result: "jkowals95");
        SetupCredentialServiceMockToReturnPassword(credentialServiceMock, result: "1$g&J*34");
        fakeDbContext.SetupToThrowOperationCanceledException();
        
        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(ApplicationIdentityDbContext)))
            .Returns(fakeDbContext);

        var authService = CreateAuthService(
            signInManagerMock,
            credentialServiceMock: credentialServiceMock,
            serviceProviderMock: serviceProviderMock
        );

        // Act
        Func<Task> result = async () => await authService.RegisterAsync(registrationRequest);

        // Assert
        await result
            .Should()
            .ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task Register_ForRegistrationCredentials_ThrowsBadRequestException_WhenCreatingUserFailed()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var signInManagerMock = SignInManagerMock.Create(userManagerMock.Object);
        var credentialServiceMock = CreateCredentialServiceMock();

        var registrationRequest = CreateRegistrationRequest();

        SetupCredentialServiceMockToReturnLogin(credentialServiceMock, result: "jkowals95");
        SetupCredentialServiceMockToReturnPassword(credentialServiceMock, result: "1$g&J*34");

        UserManagerMock.SetupCreateToReturnIdentityResult(userManagerMock, IdentityResult.Failed());

        var authService = CreateAuthService(signInManagerMock, credentialServiceMock: credentialServiceMock);

        var expectedExceptionMessage = "Cannot create a new user for given credentials";

        // Act
        Func<Task> result = async () => await authService.RegisterAsync(registrationRequest);

        // Assert
        await result
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Register_ForRegistrationCredentials_ThrowsBadRequestException_WhenAddingToRolesFailed()
    {
        // Arrange
        List<string> userRoles = ["Employee", "Manager"]; 
        var userManagerMock = UserManagerMock.Create();
        var signInManagerMock = SignInManagerMock.Create(userManagerMock.Object);
        var credentialServiceMock = CreateCredentialServiceMock();

        var registrationRequest = CreateRegistrationRequest(roles: userRoles);

        SetupCredentialServiceMockToReturnLogin(credentialServiceMock, result: "jkowals95");
        SetupCredentialServiceMockToReturnPassword(credentialServiceMock, result: "1$g&J*34");

        UserManagerMock.SetupCreateToReturnIdentityResult(userManagerMock, IdentityResult.Success);
        UserManagerMock.SetupAddToRolesToReturnIdentityResult(userManagerMock, IdentityResult.Failed());

        var authService = CreateAuthService(signInManagerMock, credentialServiceMock: credentialServiceMock);

        var expectedExceptionMessage = "Cannot add a new user to roles ['Employee, Manager']";

        // Act
        Func<Task> result = async () => await authService.RegisterAsync(registrationRequest);

        // Assert
        await result
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Register_ForRegistrationCredentials_RollbackTransaction_WhenCreatingUserFailed()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var signInManagerMock = SignInManagerMock.Create(userManagerMock.Object);
        var credentialServiceMock = CreateCredentialServiceMock();
        var serviceProviderMock = CreateServiceProviderMock();
        var dbContextTransactionMock = CreateDbContextTransactionMock();
        var fakeDbContext = CreateFakeApplicationIdentityDbContext();

        var registrationRequest = CreateRegistrationRequest();

        SetupCredentialServiceMockToReturnLogin(credentialServiceMock, result: "jkowals95");
        SetupCredentialServiceMockToReturnPassword(credentialServiceMock, result: "1$g&J*34");

        fakeDbContext.SetupSetupToReturnDbContextTransaction(dbContextTransactionMock.Object);

        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(ApplicationIdentityDbContext)))
            .Returns(fakeDbContext);

        UserManagerMock.SetupCreateToReturnIdentityResult(userManagerMock, IdentityResult.Failed());

        var authService = CreateAuthService(
            signInManagerMock,
            credentialServiceMock: credentialServiceMock,
            serviceProviderMock: serviceProviderMock
        );

        // Act 
        Func<Task> result = async () => await authService.RegisterAsync(registrationRequest);

        // Assert
        await result
            .Should()
            .ThrowAsync<BadRequestException>();

        dbContextTransactionMock
            .Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Register_ForRegistrationCredentials_RollbackTransaction_WhenAddToRolesFailed()
    {
        // Arrange
        List<string> userRoles = ["Employee", "Manager"];
        var userManagerMock = UserManagerMock.Create();
        var signInManagerMock = SignInManagerMock.Create(userManagerMock.Object);
        var credentialServiceMock = CreateCredentialServiceMock();
        var serviceProviderMock = CreateServiceProviderMock();
        var dbContextTransactionMock = CreateDbContextTransactionMock();
        var fakeDbContext = CreateFakeApplicationIdentityDbContext();

        var registrationRequest = CreateRegistrationRequest(roles: userRoles);

        SetupCredentialServiceMockToReturnLogin(credentialServiceMock, result: "jkowals95");
        SetupCredentialServiceMockToReturnPassword(credentialServiceMock, result: "1$g&J*34");

        fakeDbContext.SetupSetupToReturnDbContextTransaction(dbContextTransactionMock.Object);

        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(ApplicationIdentityDbContext)))
            .Returns(fakeDbContext);

        UserManagerMock.SetupCreateToReturnIdentityResult(userManagerMock, IdentityResult.Success);
        UserManagerMock.SetupAddToRolesToReturnIdentityResult(userManagerMock, IdentityResult.Failed());

        var authService = CreateAuthService(
            signInManagerMock,
            credentialServiceMock: credentialServiceMock,
            serviceProviderMock: serviceProviderMock
        );

        // Act
        Func<Task> result = async () => await authService.RegisterAsync(registrationRequest);

        // Assert
        await result
            .Should()
            .ThrowAsync<BadRequestException>();

        dbContextTransactionMock
            .Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
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

        SetupCredentialServiceMockToReturnLogin(credentialServiceMock, result: "jkowals95");
        SetupCredentialServiceMockToReturnPassword(credentialServiceMock, result: "1$g&J*34");

        UserManagerMock.SetupCreateToReturnIdentityResult(userManagerMock, IdentityResult.Success);
        UserManagerMock.SetupAddToRolesToReturnIdentityResult(userManagerMock, IdentityResult.Success);

        userManagerMock
            .Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(token);

        emailServiceMock
            .Setup(es => es.GenerateEmailConfirmationLink(It.IsAny<string>(), token, CancellationToken.None))
            .Returns(confirmationLink);

        var authService = CreateAuthService(signInManagerMock, credentialServiceMock, emailServiceMock);

        // Act
        var registrationResponse = await authService.RegisterAsync(registrationRequest);

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
        Func<Task> result = async () => await authService.ConfirmEmailAsync(userId, token);

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

        UserManagerMock.SetupToFindUserById(userManagerMock, result: null);

        var authService = CreateAuthService(signInManagerMock);
        var expectedExceptionMessage = "No user with ID: user_id found";

        // Act
        Func<Task> result = async () => await authService.ConfirmEmailAsync(userId, token);

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

        UserManagerMock.SetupToFindUserById(userManagerMock, result: user);
        UserManagerMock.SetupIsEmailConfirmedToReturnResult(userManagerMock, result: true);

        var authService = CreateAuthService(signInManagerMock);
        var expectedExceptionMessage = "Email for jkowalski95@company.com is already confirmed";

        // Act
        Func<Task> result = async () => await authService.ConfirmEmailAsync(userId, token);

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

        UserManagerMock.SetupToFindUserById(userManagerMock, result: user);
        UserManagerMock.SetupIsEmailConfirmedToReturnResult(userManagerMock, result: false);
        UserManagerMock.SetupConfirmEmailAsyncToReturnIdentityResult(userManagerMock, IdentityResult.Failed());

        var authService = CreateAuthService(signInManagerMock);
        var expectedExceptionMessage = "Failed to confirm email for jkowalski95@company.com";

        // Act
        Func<Task> result = async () => await authService.ConfirmEmailAsync(userId, token);

        // Assert
        await result
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task ConfirmEmail_ForGivenUserIdAndToken_ConfirmsUserEmailSuccessfully()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var signInManagerMock = SignInManagerMock.Create(userManagerMock.Object);

        string userId = "user_id";
        string token = "token";
        var user = CreateAplicationUser();

        UserManagerMock.SetupToFindUserById(userManagerMock, result: user);
        UserManagerMock.SetupIsEmailConfirmedToReturnResult(userManagerMock, result: false);
        UserManagerMock.SetupConfirmEmailAsyncToReturnIdentityResult(userManagerMock, IdentityResult.Success);

        var authService = CreateAuthService(signInManagerMock);

        // Act
        await authService.ConfirmEmailAsync(userId, token);

        // Assert
        userManagerMock
            .Verify(um => um.ConfirmEmailAsync(user, token), Times.Once);
    }

    [Fact]
    public async Task ChangePassword_ThrowsNotFoundException_WhenUserContextNotFound()
    {
        // Arrange
        var newPassword = "P@ssword2";
        var passwordRequest = CreatePasswordRequest("P@ssword1", newPassword);

        var signInManagerMock = SignInManagerMock.Create();
        var userServiceMock = CreateUserServiceMock();

        SetupUserServiceMockToReturnUser(userServiceMock, result: null);

        var authService = CreateAuthService(signInManagerMock, userServiceMock: userServiceMock);

        var expectedExceptionMessage = "No user found in current context";

        // Act
        Func<Task> result = async () => await authService.ChangePasswordAsync(passwordRequest);

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

        SetupUserServiceMockToReturnUser(userServiceMock, result: new ClaimsPrincipal());
        UserManagerMock.SetupGetUserToFindByApplicationUser(userManagerMock, result: null);

        var authService = CreateAuthService(signInManagerMock, userServiceMock: userServiceMock);

        var expectedExceptionMessage = "No user found";

        // Act
        Func<Task> result = async () => await authService.ChangePasswordAsync(passwordRequest);

        // Assert
        await result
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task ChangePassword_ThrowsBadRequestException_WhenChangingPasswordFailed()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var signInManagerMock = SignInManagerMock.Create(userManagerMock.Object);
        var userServiceMock = CreateUserServiceMock();

        var currentPassword = "P@ssword1";
        var newPassword = "P@ssword2";
        var passwordRequest = CreatePasswordRequest(currentPassword, newPassword);
        var user = CreateAplicationUser();

        SetupUserServiceMockToReturnUser(userServiceMock, result: new ClaimsPrincipal());
        UserManagerMock.SetupGetUserToFindByApplicationUser(userManagerMock, result: user);

        userManagerMock
            .Setup(um => um.ChangePasswordAsync(It.IsAny<ApplicationUser>(), currentPassword, newPassword))
            .ReturnsAsync(IdentityResult.Failed());

        var authService = CreateAuthService(signInManagerMock, userServiceMock: userServiceMock);

        var expectedExceptionMessage = "Failed to change password for user jkowalski95@company.com";

        // Act
        Func<Task> result = () => authService.ChangePasswordAsync(passwordRequest);

        // Assert
        await result
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage(expectedExceptionMessage);
    }

    #region Test_Factory_Methods

    private static FakeApplicationIdentityDbContext CreateFakeApplicationIdentityDbContext() => new();
    private static Mock<IDbContextTransaction> CreateDbContextTransactionMock() => new();
    private static Mock<ICredentialService> CreateCredentialServiceMock() => new();
    private static Mock<IJwtService> CreateJwtServiceMock() => new();
    private static Mock<IEmailService> CreateEmailServiceMock() => new();
    private static Mock<IUserService> CreateUserServiceMock() => new();
    private static Mock<IServiceProvider> CreateServiceProviderMock() => new();

    private static void SetupCredentialServiceMockToReturnLogin(Mock<ICredentialService> credentialServiceMock,
                                                                string result)
        => credentialServiceMock
            .Setup(cs => cs.GenerateUserLogin(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(result);

    private static void SetupCredentialServiceMockToReturnPassword(Mock<ICredentialService> credentialServiceMock,
                                                                   string result)
        => credentialServiceMock
            .Setup(cs => cs.GenerateUserPassword())
            .Returns(result);

    private static void SetupUserServiceMockToReturnUser(Mock<IUserService> userServiceMock,
                                                         ClaimsPrincipal? result)
        => userServiceMock
            .Setup(us => us.User)
            .Returns(() => result);

    private static AuthRequest CreateAuthRequest() => new("jkowals95", "P@ssword1");

    private static RegistrationRequest CreateRegistrationRequest(string firstName = "Jan",
                                                                 string lastName = "Kowalski",
                                                                 string email = "jkowalski95@company.com",
                                                                 DateTime dateOfBirth = new(),
                                                                 string? peselNumber = "12345678911",
                                                                 string phoneNumber = "566889111",
                                                                 List<string>? roles = null)
       => new(firstName, lastName, email, dateOfBirth, peselNumber, phoneNumber, roles);

    private static PasswordRequest CreatePasswordRequest(string currentPassword, string newPassword)
        => new(currentPassword, newPassword);

    private static AuthService CreateAuthService(Mock<SignInManager<ApplicationUser>> signInManagerMock,
                                                 Mock<ICredentialService>? credentialServiceMock = null,
                                                 Mock<IEmailService>? emailServiceMock = null,
                                                 Mock<IJwtService>? jwtServiceMock = null,
                                                 Mock<IUserService>? userServiceMock = null,
                                                 Mock<IServiceProvider>? serviceProviderMock = null)
    {
        var identityResultMock = new Mock<IIdentityResult>();
        var loggerMock = new Mock<IAppLogger<AuthService>>();

        credentialServiceMock ??= CreateCredentialServiceMock();
        emailServiceMock ??= CreateEmailServiceMock();
        jwtServiceMock ??= CreateJwtServiceMock();
        userServiceMock ??= CreateUserServiceMock();

        if (serviceProviderMock is null)
        {
            var fakeDbContext = CreateFakeApplicationIdentityDbContext();
            var transactionMock = CreateDbContextTransactionMock();
            serviceProviderMock = CreateServiceProviderMock();

            fakeDbContext.SetupSetupToReturnDbContextTransaction(transactionMock.Object);

            serviceProviderMock
                .Setup(sp => sp.GetService(typeof(ApplicationIdentityDbContext)))
                .Returns(fakeDbContext);
        }

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
