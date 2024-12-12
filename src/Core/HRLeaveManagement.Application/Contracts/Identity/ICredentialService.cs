namespace HRLeaveManagement.Application.Contracts.Identity;

public interface ICredentialService
{
    string GenerateUserLogin(string firstname, string lastname, string dateOfBirth);
    string GenerateUserPassword();
}
