using HRLeaveManagement.BlazorUI.Services.Base;
using System.ComponentModel;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.ViewModels.Employees;

public class EmployeeContractViewModel
{
    public int? Id { get; set; }

    [DisplayName("Contract type")]
    public ContractType? ContractType { get; set; }

    [DisplayName("Start date")]
    public DateTime? StartedAt { get => EmploymentDateRange?.Start; set => EmploymentDateRange.Start = value; }

    [DisplayName("Expiration date")]
    public DateTime? ExpiredAt { get => EmploymentDateRange?.End; set => EmploymentDateRange.End = value; }

    [DisplayName("Contract duration")]
    public int? TotalDuration { get; set; }

    [DisplayName("Creation date")]
    public DateTime? CreatedAt { get; set; }

    [DisplayName("Modification date")]
    public DateTime? ModifiedAt { get; set; }

    public DateRange? EmploymentDateRange { get; set; } = new();

    public bool IsContractForIndefinitePeriod { get; set; }

    public string StartedAtShortDate => StartedAt.HasValue ? StartedAt.Value.ToShortDateString() : string.Empty;
    public string ExpiredAtShortDate => ExpiredAt.HasValue ? ExpiredAt.Value.ToShortDateString() : string.Empty;

    public string ContractDescription => StartedAt.HasValue && StartedAt <= DateTime.Now
        ? !ExpiredAt.HasValue || ExpiredAt >= DateTime.Now
            ? "currently ongoing"
            : $"expired {(DateTime.Now - ExpiredAt).Value.Days} days ago"
        : $"valid for {(StartedAt - DateTime.Now).Value.Days} days";

    public Dictionary<ContractType?, string> ContractTypes = new()
    {
        { Services.Base.ContractType._0, "Employment contract" },
        { Services.Base.ContractType._1, "B2B" }
    };
}
