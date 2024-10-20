using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout;

public partial class Error
{
    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Inject]
    protected ISnackbar SnackbarService { get; set; }

    public void HandleError(string message)
    {
        SnackbarService.Configuration.SnackbarVariant = Variant.Filled;
        SnackbarService.Configuration.NewestOnTop = false;
        SnackbarService.Configuration.HideTransitionDuration = 500;
        SnackbarService.Configuration.ShowTransitionDuration = 500;

        SnackbarService.Add(message, Severity.Error);

        Console.WriteLine($"{ message } at { DateTime.Now }");
    }
}