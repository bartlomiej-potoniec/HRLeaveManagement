using HRLeaveManagement.BlazorUI.Services.Base;

namespace HRLeaveManagement.BlazorUI.ViewModels;

public class EmployeeExperienceDetailsViewModel
{
    public int? Id { get; set; }
    public ContractType ContractType { get; set; }
    public string PreviousCompanyName { get; set; }
    public string Position { get; set; }
    public DateTime? EmployedFrom { get; set; }
    public DateTime? EmployedTo { get; set; }
    public int TotalEmployment { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

    public Dictionary<ContractType, string> ContractTypes = new()
    {
        { Services.Base.ContractType._0, "Umowa o pracę" },
        { Services.Base.ContractType._1, "B2B" }
    };
}
