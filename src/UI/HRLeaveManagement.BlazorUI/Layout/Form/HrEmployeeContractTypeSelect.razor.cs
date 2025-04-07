using HRLeaveManagement.BlazorUI.Services.Base;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace HRLeaveManagement.BlazorUI.Layout.Form;

public partial class HrEmployeeContractTypeSelect
{
    [Parameter, EditorRequired]
    public required ContractType? ContractType { get; set; }

    [Parameter, EditorRequired]
    public required Dictionary<ContractType?, string> ContractTypes { get; set; } = [];

    [Parameter] public Expression<Func<ContractType?>> For { get; set; }
    [Parameter] public EventCallback<ContractType?> ContractTypeChanged { get; set; }
    [Parameter] public bool IsDisabled { get; set; } = false;
}
