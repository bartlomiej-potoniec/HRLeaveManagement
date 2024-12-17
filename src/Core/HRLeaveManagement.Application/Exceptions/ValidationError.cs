namespace HRLeaveManagement.Application.Exceptions;

public class ValidationError(string key, string[] messages)
{
    public string Key => key;
    public string[] Messages => messages;
}
