using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.Sections;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace HRLeaveManagement.BlazorUI.Layout.Form;

public partial class HrSectionSelect: ComponentBase
{
    [Inject] private ISectionService SectionService { get; set; }

    [CascadingParameter] protected Message Message { get; set; }

    [Parameter, EditorRequired] public required int? Value { get; set; }
    [Parameter, EditorRequired] public required Expression<Func<int?>> For { get; set; }
    [Parameter] public EventCallback<int?> ValueChanged { get; set; }
    [Parameter] public EventCallback<bool> OnDataLoaded { get; set; }
    [Parameter] public bool IsDisabled { get; set; } = false;

    private List<SectionViewModel> Sections { get; set; } = [];

    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        var response = await SectionService.GetAllAsync();

        if (!response.IsSuccess)
        {
            Message.HandleError(response.Message);
            return;
        }

        Sections = response.Data;

        _isLoading = false;
        await OnDataLoaded.InvokeAsync(true);
    }
}
