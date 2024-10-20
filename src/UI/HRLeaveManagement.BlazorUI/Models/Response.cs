namespace HRLeaveManagement.BlazorUI.Models;

public record Response<T>
{
    public required string Message { get; init; }
    public T? Data { get; init; }

    public bool IsSuccess { get; init; } = false;
    public List<string> ValidationErrors { get; init; } = [];
}
