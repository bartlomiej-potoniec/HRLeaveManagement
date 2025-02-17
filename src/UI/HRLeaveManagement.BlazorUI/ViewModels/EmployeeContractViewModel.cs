using HRLeaveManagement.BlazorUI.Services.Base;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.ViewModels;

public class EmployeeContractViewModel
{
    public int? Id { get; set; }
    public ContractType? ContractType { get; set; }

    public DateRange? EmploymentDateRange { get; set; } = new();
    public DateTime? EmployedFrom { get => EmploymentDateRange?.Start; set => EmploymentDateRange.Start = value; }
    public DateTime? EmployedTo { get => EmploymentDateRange?.End; set => EmploymentDateRange.End = value; }
   
    public int? TotalDuration { get; set; }

    public string ContractDescription => EmployedTo.HasValue
        ? EmployedFrom > DateTime.Now 
            ? $"obowiązuje za {(EmployedFrom - DateTime.Now).Value.Days} dni"
            : $"wygasła {(DateTime.Now - EmployedTo).Value.Days} dni temu"
        : "aktualnie trwa";

    public Dictionary<ContractType?, string> ContractTypes = new()
    {
        { Services.Base.ContractType._0, "Umowa o pracę" },
        { Services.Base.ContractType._1, "B2B" }
    };
}
