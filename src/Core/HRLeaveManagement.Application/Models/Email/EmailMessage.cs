namespace HRLeaveManagement.Application.Models.Email;

public sealed class EmailMessage
{
    public required string To { get; set; }
    public required string Subject { get; set; }
    public required string TextContent { get; set; }
}