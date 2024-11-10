namespace HRLeaveManagement.Identity.Options;

public class CredentialOptions
{
    public required LoginOptions Login { get; set; }
    public required PasswordOptions Password { get; set; }
}

public class LoginOptions
{
    public required short Length { get; set; }
    public required short MinRandomValue { get; set; }
    public required short MaxRandomValue { get; set; }
}

public class PasswordOptions
{
    public required short Length { get; set; }
    public required string AllowedSpecialChars { get; set; }
}