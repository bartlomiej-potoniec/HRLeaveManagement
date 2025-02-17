using HRLeaveManagement.BlazorUI.Services.Base;

namespace HRLeaveManagement.BlazorUI.ViewModels;

public class EmployeeEducationViewModel
{
    public int? Id { get; set; }
    public EducationType? EducationType { get; set; } = null;
    public string EducationDetails { get; set; }
    public DateTime? EnrolledAt { get; set; }
    public DateTime? GraduatedAt { get; set; }

    public Dictionary<EducationType?, string> EducationTypes = new()
    {
        { Services.Base.EducationType._1, "Wykształcenie podstawowe" },
        { Services.Base.EducationType._2, "Wykształcenie średnie" },
        { Services.Base.EducationType._3, "Wykształcenie średnie ogólnokształcące" },
        { Services.Base.EducationType._4, "Wykształcenie policealne" },
        { Services.Base.EducationType._5, "Wykształcenie wyższe" },
    };
}
