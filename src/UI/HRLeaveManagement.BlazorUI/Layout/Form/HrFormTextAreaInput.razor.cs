using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout.Form;

public partial class HrFormTextAreaInput<T> : ComponentBase where T : notnull
{
    [Parameter, EditorRequired] public required T Value { get; set; }
    [Parameter] public EventCallback<T> ValueChanged { get; set; }
    [Parameter] public Expression<Func<T>>? For { get; set; }

    [Parameter, EditorRequired] public int Lines { get; set; }
    [Parameter, EditorRequired] public required string Label { get; set; }
    [Parameter, EditorRequired] public required string InputId { get; set; }
    [Parameter] public Variant Variant { get; set; } = Variant.Outlined;
    [Parameter] public Margin Margin { get; set; } = Margin.Dense;
    [Parameter] public string MinLabelWith { get; set; } = "230px";
    [Parameter] public bool IsDisabled { get; set; } = false;
    [Parameter] public bool IsReadOnly { get; set; } = false;

    private bool IsClearable => !IsReadOnly;
}

