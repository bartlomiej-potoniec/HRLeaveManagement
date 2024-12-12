namespace HRLeaveManagement.Application.DTOs.Email;

public sealed class EmailMessage
{
    public required string To { get; set; }
    public required string Subject { get; set; }
    public string? TextContent { get; set; }
    public string? TemplateId { get; set; }
    public object? TemplatePlaceholders { get; set; }
}