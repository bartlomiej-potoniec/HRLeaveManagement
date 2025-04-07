using HRLeaveManagement.BlazorUI.Models;
using HRLeaveManagement.BlazorUI.ViewModels.Sections;

namespace HRLeaveManagement.BlazorUI.Contracts;

public interface ISectionService
{
    Task<Response<List<SectionViewModel>>> GetAllAsync();
}
