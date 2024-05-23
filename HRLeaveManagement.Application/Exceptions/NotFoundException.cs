namespace HRLeaveManagement.Application.Exceptions;

public class NotFoundException : Exception
{
    internal NotFoundException(string message) : base(message) {}
    internal NotFoundException(string name, object key) 
        : base($"{ name } ({ key }) was not found") {}
}
