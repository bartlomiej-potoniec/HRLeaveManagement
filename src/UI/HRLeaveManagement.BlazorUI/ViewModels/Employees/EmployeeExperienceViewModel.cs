using HRLeaveManagement.BlazorUI.Services.Base;
using MudBlazor;
using System.ComponentModel;

namespace HRLeaveManagement.BlazorUI.ViewModels.Employees;

public class EmployeeExperienceViewModel
{
    public int? Id { get; set; }

    [DisplayName("Previous company name")]
    public string? PreviousCompanyName { get; set; }

    [DisplayName("Position at previous company")]
    public string? Position { get; set; }

    [DisplayName("Contract type")]
    public ContractType? ContractType { get; set; }

    [DisplayName("Employed from")]
    public DateTime? EmployedFrom { get => EmploymentDateRange.Start; set => EmploymentDateRange.Start = value; }

    [DisplayName("Employed to")]
    public DateTime? EmployedTo { get => EmploymentDateRange.End; set => EmploymentDateRange.End = value; }

    [DisplayName("Employment period")]
    public int TotalEmployment { get; set; }

    [DisplayName("Creation date")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Modification date")]
    public DateTime ModifiedAt { get; set; }

    public DateRange EmploymentDateRange { get; set; } = new();

    public string EmployedFromShortDate => EmployedFrom.HasValue ? EmployedFrom.Value.ToShortDateString() : string.Empty;
    public string EmployedToShortDate => EmployedTo.HasValue ? EmployedTo.Value.ToShortDateString() : string.Empty;

    public DateTime MaxEmployedToDate => DateTime.UtcNow;

    public Dictionary<ContractType?, string> ContractTypes = new()
    {
        { Services.Base.ContractType._0, "Employment contract" },
        { Services.Base.ContractType._1, "B2B" }
    };
}
