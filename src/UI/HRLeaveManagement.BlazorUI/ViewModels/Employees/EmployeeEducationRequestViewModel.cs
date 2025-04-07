using HRLeaveManagement.BlazorUI.Services.Base;
using System.ComponentModel;

namespace HRLeaveManagement.BlazorUI.ViewModels.Employees;

public class EmployeeEducationViewModel
{
    public int? Id { get; set; }

    [DisplayName("Education type")]
    public EducationType? EducationType { get; set; } = default;

    [DisplayName("Education details")]
    public string? EducationDetails { get; set; }

    [DisplayName("Enroll date")]
    public DateTime? EnrolledAt { get; set; }

    [DisplayName("Graduation date")]
    public DateTime? GraduatedAt { get; set; }

    [DisplayName("Creation date")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Modification date")]
    public DateTime ModifiedAt { get; set; }

    public bool IsEmployeeStillStudying { get; set; } = false;
    public string EducationTypeName => EducationTypes[EducationType];

    public string EnrolledAtShortDate => EnrolledAt.HasValue ? EnrolledAt.Value.ToShortDateString() : string.Empty;
    public string GraduatedAtShortDate => GraduatedAt.HasValue ? GraduatedAt.Value.ToShortDateString() : string.Empty;

    public Dictionary<EducationType?, string> EducationTypes = new()
    {
        { Services.Base.EducationType._1, "Basic education" },
        { Services.Base.EducationType._2, "Secondary education" },
        { Services.Base.EducationType._3, "General secondary education" },
        { Services.Base.EducationType._4, "Post secondary education" },
        { Services.Base.EducationType._5, "Higher education" },
    };
}
