using HRLeaveManagement.Identity.Models;
using HRLeaveManagement.Identity.Options;
using HRLeaveManagement.Identity.Services;
using Microsoft.Extensions.Options;

namespace HRLeaveManagement.Identity.Tests.Services;

public class CredentialServiceTest
{
    [Theory]
    [InlineData("John", "Doe", "19900605", "jdoe90")]
    [InlineData("Bartosz", "Kowalski", "19981112", "bkowals98")]
    [InlineData("Joanna", "Wika-Nowak", "20010523", "jwikan01")]
    [InlineData("Catharine", "Smiths-Doe", "19990303", "csmiths99")]
    public void GenerateUserLogin_ForGivenCredentials_GeneratesAppropriateLoginFormat_WhenNoSimilarCredentialsExist(string firstname,
                                                                                                                    string lastname,
                                                                                                                    string dateOfBirth,
                                                                                                                    string expectedLogin)
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var credentialOptionsMock = CreateCredentialOptionsMock();

        var credentialService = new CredentialService(userManagerMock.Object, credentialOptionsMock.Object);

        // Act
        var login = credentialService.GenerateUserLogin(firstname, lastname, dateOfBirth);

        // Assert
        login
            .Should()
            .Be(expectedLogin);
    }

    [Theory]
    [InlineData("John", "Kowalski", "19950605")]
    [InlineData("Jan", "Kowalski", "19951112")]
    [InlineData("Joanna", "Kowalska", "19950523")]
    [InlineData("Joseph", "Kowalski", "19950303")]
    public void GenerateUserLogin_ForGivenCredentials_GeneratesAppropriateLoginFormat_WhenLoginForSimilarCredentialsExist(string firstname,
                                                                                                                          string lastname,
                                                                                                                          string dateOfBirth)
    {
        // Arrange
        List<ApplicationUser> users = [
            new() { UserName = "jkowals95", FirstName = "Jadwiga", LastName = "Kowalska", DateOfBirth = new(1995, 7, 12) },
            new() { UserName = "jkowals70", FirstName = "Jurek", LastName = "Kowalski", DateOfBirth = new(1995, 4, 24) },
            new() { UserName = "jkowals05", FirstName = "Jagoda", LastName = "Kowalska", DateOfBirth = new(1995, 2, 16) },
        ];

        List<string> userLogins = [users[0].UserName!, users[1].UserName!, users[2].UserName!];

        var userManagerMock = UserManagerMock.Create(users.AsQueryable());
        var credentialOptionsMock = CreateCredentialOptionsMock();

        var credentialService = new CredentialService(userManagerMock.Object, credentialOptionsMock.Object);

        // Act
        var login = credentialService.GenerateUserLogin(firstname, lastname, dateOfBirth);

        // Assert
        userLogins
            .Should()
            .NotContain(login);
    }

    [Fact]
    public void GenerateUserPassword_ForGivenAppSettingsOptions_ReturnsRandomPasswordForGivenLength()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var credentialOptionsMock = CreateCredentialOptionsMock();

        var credentialService = new CredentialService(userManagerMock.Object, credentialOptionsMock.Object);

        // Act
        var password = credentialService.GenerateUserPassword();

        // Assert
        password.Length
            .Should()
            .Be(12);
    }

    [Fact]
    public void GenerateUserPassword_ForGivenAppSettingsOptions_ReturnsRandomPasswordContainingAllowedSpecialChars()
    {
        // Arrange
        var userManagerMock = UserManagerMock.Create();
        var credentialOptionsMock = CreateCredentialOptionsMock();

        var credentialService = new CredentialService(userManagerMock.Object, credentialOptionsMock.Object);

        // Act
        var password = credentialService.GenerateUserPassword();

        // Assert
        password
            .Should()
            .ContainAny("!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "~");
    }

    #region Test_Factory_Methods

    private static void SetupCredentialOptionsMockToReturnCredentialOptions(Mock<IOptions<CredentialOptions>> credentialOptionsMock,
                                                                            CredentialOptions credentialOptions)
        => credentialOptionsMock
            .Setup(o => o.Value)
            .Returns(credentialOptions);

    private static Mock<IOptions<CredentialOptions>> CreateCredentialOptionsMock()
    {
        var credentialOptions = CreateCredentialOptions();
        var credentialOptionsMock = new Mock<IOptions<CredentialOptions>>();
        
        SetupCredentialOptionsMockToReturnCredentialOptions(credentialOptionsMock, credentialOptions);

        return credentialOptionsMock;
    }

    private static CredentialOptions CreateCredentialOptions()
        => new()
        {
            Login = new LoginOptions { Length = 9, MinRandomValue = 1, MaxRandomValue = 99 },
            Password = new PasswordOptions { Length = 12, AllowedSpecialChars = "!@#$%^&*()_~" }
        };

    #endregion
}
