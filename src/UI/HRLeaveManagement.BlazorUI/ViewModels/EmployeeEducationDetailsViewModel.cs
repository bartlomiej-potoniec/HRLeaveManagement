using HRLeaveManagement.BlazorUI.Services.Base;

namespace HRLeaveManagement.BlazorUI.ViewModels;

public class EmployeeEducationDetailsViewModel
{
    public int? Id { get; set; }
    public EducationType EducationType { get; set; }
    public string EducationDetails { get; set; }
    public DateTime? EnrolledAt { get; set; }
    public DateTime? GraduatedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

    public Dictionary<EducationType, string> EducationTypes = new()
    {
        { EducationType._1, "Wykształcenie podstawowe" },
        { EducationType._2, "Wykształcenie średnie" },
        { EducationType._3, "Wykształcenie średnie ogólnokształcące" },
        { EducationType._4, "Wykształcenie policealne" },
        { EducationType._5, "Wykształcenie wyższe" },
    };
}
