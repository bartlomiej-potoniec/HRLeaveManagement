using HRLeaveManagement.BlazorUI.ViewModels.Sections;

namespace HRLeaveManagement.BlazorUI.Contracts;

public interface ISectionService
{
    Task<List<SectionViewModel>> GetAllAsync();
}
