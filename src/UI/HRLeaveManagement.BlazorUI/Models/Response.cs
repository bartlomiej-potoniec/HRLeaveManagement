namespace HRLeaveManagement.BlazorUI.Models;

public record Response
{
    public required string Message { get; init; }
    public bool IsSuccess { get; init; } = false;
    public List<string> ValidationErrors { get; init; } = [];
}

public record Response<TData> : Response
{
    public TData? Data { get; init; }
}
