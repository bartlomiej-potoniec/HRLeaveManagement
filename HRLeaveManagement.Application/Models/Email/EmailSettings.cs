namespace HRLeaveManagement.Application.Models.Email;

public sealed class EmailSettings
{
    public required string ApiKey { get; set; }
    public required string FromAddress { get; set; }
    public required string FromName { get; set; }
}