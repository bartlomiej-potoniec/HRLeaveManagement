using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout.Dialog;

public partial class HrDialog : ComponentBase
{
    [CascadingParameter] protected MudDialogInstance MudDialog { get; set; }

    [Parameter, EditorRequired] public required RenderFragment ChildBodyContent { get; set; }

    [Parameter] public EventCallback OnConfirm { get; set; }
    [Parameter] public RenderFragment? ChildDescriptionContent { get; set; }
    [Parameter] public string Icon { get; set; } = Icons.Material.Filled.Info;
    [Parameter] public Color ButtonColor { get; set; } = Color.Primary;

    protected string Title => MudDialog.Title is null ? "Form" : MudDialog.Title;

    protected override void OnInitialized()
    {
        MudDialog.Options.CloseButton = true;
        MudDialog.Options.CloseOnEscapeKey = true;
        MudDialog.Options.BackdropClick = false;
        MudDialog.Options.FullWidth = true;
        MudDialog.Options.Position = DialogPosition.Center;

        MudDialog.SetOptions(MudDialog.Options);
    }

    protected virtual async Task Confirm() 
    {
        MudDialog.Close(DialogResult.Ok(true));
        await Task.CompletedTask;
    }

    private async Task ConfirmHandler()
    {
        if (OnConfirm.HasDelegate)
        {
            await OnConfirm.InvokeAsync();
            return;
        }

        await Confirm();
    }

    protected void Cancel() => MudDialog.Cancel();
}
