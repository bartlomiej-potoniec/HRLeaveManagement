using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout;

public partial class Message
{
    [Parameter] public RenderFragment ChildContent { get; set; }

    [Inject] protected ISnackbar SnackbarService { get; set; }

    public void HandleSuccess(string message) => HandleMessage(message, Severity.Success);
    public void HandleInfo(string message) => HandleMessage(message, Severity.Info);
    public void HandleWarning(string message) => HandleMessage(message, Severity.Warning);
    public void HandleError(string message) => HandleMessage(message, Severity.Error);
    public void HandleError(IEnumerable<string> errors) => errors
        .ToList()
        .ForEach(error => HandleMessage(error, Severity.Error));
    
    private void HandleMessage(string message, Severity severity = Severity.Normal)
    {
        SnackbarService.Configuration.SnackbarVariant = Variant.Filled;
        SnackbarService.Configuration.NewestOnTop = false;
        SnackbarService.Configuration.HideTransitionDuration = 200;
        SnackbarService.Configuration.ShowTransitionDuration = 200;
        SnackbarService.Add(message, severity);

        Console.WriteLine($"{ message } at { DateTime.Now }");
    }
}