using HRLeaveManagement.BlazorUI.Services.Base;

namespace HRLeaveManagement.BlazorUI.ViewModels;

public class EmployeeContractDetailsViewModel
{
    public int? Id { get; set; }
    public ContractType ContractType { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? ExpiredAt { get; set; }
    public int? TotalDuration { get; set; }
    public string ContractDescription => ExpiredAt.HasValue
        ? $"Wygasła { (DateTime.Now - ExpiredAt).Value.Days } dni temu"
        : "aktualnie trwa";

    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

    public Dictionary<ContractType, string> ContractTypes = new()
    {
        { ContractType._0, "Umowa o pracę" },
        { ContractType._1, "B2B" }
    };
}
