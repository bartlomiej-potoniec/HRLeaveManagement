namespace HRLeaveManagement.Infrastructure.Email.Options;

public sealed class EmailOptions
{
    public required string ApiKey { get; set; }
    public required string FromAddress { get; set; }
    public required string FromName { get; set; }
    public Dictionary<string, string> TemplatesId { get; set; } = [];
}