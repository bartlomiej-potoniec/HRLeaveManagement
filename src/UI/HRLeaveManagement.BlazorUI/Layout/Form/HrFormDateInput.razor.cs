using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout.Form;

public partial class HrFormDateInput : ComponentBase
{
    [Parameter, EditorRequired] public required DateTime? Date { get; set; }
    [Parameter] public EventCallback<DateTime?> DateChanged { get; set; }
    [Parameter] public Expression<Func<DateTime?>> For { get; set; }

    [Parameter, EditorRequired] public required string Label { get; set; }
    [Parameter, EditorRequired] public required string InputId { get; set; }
    [Parameter] public Variant Variant { get; set; } = Variant.Outlined;
    [Parameter] public Margin Margin { get; set; } = Margin.Dense;
    [Parameter] public int Xs { get; set; } = 12;

    [Parameter] public bool IsReadOnly { get; set; } = false;
    [Parameter] public bool IsDisabled { get; set; } = false;

    private bool IsClearable => !IsReadOnly;
}
