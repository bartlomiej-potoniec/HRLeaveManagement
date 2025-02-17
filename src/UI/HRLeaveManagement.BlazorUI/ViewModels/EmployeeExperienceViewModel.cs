using HRLeaveManagement.BlazorUI.Services.Base;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.ViewModels;

public class EmployeeExperienceViewModel
{
    public int? Id { get; set; }
    public string PreviousCompanyName { get; set; }
    public string Position { get; set; }
    public ContractType? ContractType { get; set; }

    public DateRange? EmploymentDateRange { get; set; } = new();
    public DateTime? EmployedFrom { get => EmploymentDateRange?.Start; set => EmploymentDateRange.Start = value; }
    public DateTime? EmployedTo { get => EmploymentDateRange?.End; set => EmploymentDateRange.End = value; }

    public Dictionary<ContractType?, string> ContractTypes = new()
    {
        { Services.Base.ContractType._0, "Umowa o pracę" },
        { Services.Base.ContractType._1, "B2B" }
    };
}
