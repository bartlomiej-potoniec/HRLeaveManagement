using HRLeaveManagement.BlazorUI.ViewModels.Employees;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Layout.Form;

public partial class HrContractForIndefinitePeriodCheckBox : ComponentBase
{
    [Parameter, EditorRequired]
    public required EmployeeContractViewModel Contract { get; set; }

    [Parameter, EditorRequired]
    public required EventCallback<EmployeeContractViewModel> ContractChanged { get; set; }

    [Parameter] public bool IsDisabled { get; set; } = false;

    private async Task ContractForIndefinitePeriodChangedAsync(bool value)
    {
        if (!value)
        {
            Contract.IsContractForIndefinitePeriod = false;
            await ContractChanged.InvokeAsync(Contract);

            return;
        }

        Contract.IsContractForIndefinitePeriod = true;
        Contract.ExpiredAt = null;

        await ContractChanged.InvokeAsync(Contract);
    }
}
