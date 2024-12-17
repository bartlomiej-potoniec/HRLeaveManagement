using AutoMapper;
using HRLeaveManagement.Application.DTOs.Departments;
using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.MappingProfiles;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<Department, DepartmentDTO>().ReverseMap();
        CreateMap<Department, DepartmentDetailsDTO>().ReverseMap();
    }
}
