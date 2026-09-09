using HRLeaveManagement.Application.DTOs.Employees;
using HRLeaveManagement.Application.DTOs.Users;
using HRLeaveManagement.Application.MappingResolvers;
using AutoMapper;
using HRLeaveManagement.Domain.Employee;
using HRLeaveManagement.Domain.Employee.Contract;
using HRLeaveManagement.Domain.Employee.Education;
using HRLeaveManagement.Domain.Employee.Experience;

namespace HRLeaveManagement.Application.MappingProfiles;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<(UserDTO userDto, Employee employee), EmployeeDTO>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.userDto.Id))
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.employee.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.userDto.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.userDto.PhoneNumber))
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.employee.Position))
            .ForMember(dest => dest.Section, opt => opt.MapFrom(src => src.employee.Section.Name))
            .ForMember(dest => dest.Responsibilities, opt => opt.MapFrom(src => src.employee.Responsibilities))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.employee.Section.Department.Name))
            .ForMember(dest => dest.Contracts, opt => opt.MapFrom(src => src.employee.EmployeeContracts
                .Select(ec => new EmployeeContractResponse(
                    ec.Id, ec.ContractType, 
                    ec.StartedAt.ToDateTime(new TimeOnly()), 
                    ec.ExpiredAt.HasValue 
                        ? ec.ExpiredAt.Value.ToDateTime(new TimeOnly()) 
                        : null
                    )
                )
            ))
            .ForMember(dest => dest.IsCurrentlyEmployed, opt => opt.MapFrom<IsCurrentlyEmployedResolver>())
            .ForMember(dest => dest.LeaderId, opt => opt.MapFrom(src => src.employee.LeaderId))
            .ForMember(dest => dest.IsLeader, opt => 
                opt.MapFrom((src, dest, destMember, context) => 
                    context.Items.TryGetValue("IsLeader", out object? value) ? value as bool? : null))
            .ForMember(dest => dest.LeaderName, opt => 
                opt.MapFrom((src, dest, destMember, context) => 
                    context.Items.TryGetValue("LeaderName", out object? value) ? value as string : null))
            .ReverseMap();

        CreateMap<(UserDTO userDto, Employee employee), EmployeeDetailsDTO>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.userDto.Id))
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.employee.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.userDto.Email))
            .ForMember(dest => dest.PeselNumber, opt => opt.MapFrom(src => src.userDto.PeselNumber))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.userDto.PhoneNumber))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.userDto.DateOfBirth))
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.employee.Position))
            .ForMember(dest => dest.SectionId, opt => opt.MapFrom(src => src.employee.Section.Id))
            .ForMember(dest => dest.Section, opt => opt.MapFrom(src => src.employee.Section.Name))
            .ForMember(dest => dest.Responsibilities, opt => opt.MapFrom(src => src.employee.Responsibilities))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.employee.Section.Department.Id))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.employee.Section.Department.Name))
            .ForMember(dest => dest.Contracts, opt => opt.MapFrom(src => src.employee.EmployeeContracts
                .Select(ec => new EmployeeContractDetailsDTO
                {
                    Id = ec.Id,
                    ContractType = ec.ContractType,
                    StartedAt = ec.StartedAt.ToDateTime(new TimeOnly()),
                    ExpiredAt = ec.ExpiredAt.HasValue 
                        ? ec.ExpiredAt.Value.ToDateTime(new TimeOnly()) 
                        : null,
                    TotalDuration = ec.TotalDuration,
                    CreatedAt = ec.CreatedAt,
                    ModifiedAt = ec.ModifiedAt
                })
                .ToList()
            ))
            .ForMember(dest => dest.Educations, opt => opt.MapFrom(src => src.employee.EmployeeEducations
                .Select(ee => new EmployeeEducationDetailsDTO 
                {
                    Id = ee.Id,
                    EducationType = ee.EducationType,
                    EducationDetails = ee.EducationDetails,
                    EnrolledAt = ee.EnrolledAt.ToDateTime(new TimeOnly()),
                    GraduatedAt = ee.GraduatedAt.HasValue 
                        ? ee.GraduatedAt.Value.ToDateTime(new TimeOnly())
                        : null,
                    CreatedAt = ee.CreatedAt,
                    ModifiedAt = ee.ModifiedAt
                })
                .ToList()
            ))
            .ForMember(dest => dest.Experiences, opt => opt.MapFrom(src => src.employee.EmployeeExperiences
                .Select(ee => new EmployeeExperienceDetailsDTO 
                {
                    Id = ee.Id,
                    ContractType = ee.ContractType,
                    PreviousCompanyName = ee.PreviousCompanyName,
                    Position = ee.Position,
                    EmployedFrom = ee.EmployedFrom.ToDateTime(new TimeOnly()),
                    EmployedTo = ee.EmployedTo.ToDateTime(new TimeOnly()),
                    TotalEmployment = ee.TotalEmployment,
                    CreatedAt = ee.CreatedAt,
                    ModifiedAt = ee.ModifiedAt
                })
                .ToList()
            ))
            .ForMember(dest => dest.IsCurrentlyEmployed, opt => opt.MapFrom<IsCurrentlyEmployedResolver>())
            .ForMember(dest => dest.LeaderId, opt => opt.MapFrom(src => src.employee.LeaderId))
            .ForMember(dest => dest.LeaderName, opt =>
                opt.MapFrom((src, dest, destMember, context) =>
                    context.Items.TryGetValue("LeaderName", out object? value) ? value as string : null))
            .ReverseMap();

        CreateMap<EmployeeContract, EmployeeContractRequest>().ReverseMap();
        CreateMap<EmployeeEducation, EmployeeEducationRequest>().ReverseMap();
        CreateMap<EmployeeExperience, EmployeeExperienceRequest>().ReverseMap();

        CreateMap<EmployeeContract, EmployeeContractResponse>().ReverseMap();
        CreateMap<EmployeeEducation, EmployeeEducationResponse>().ReverseMap();
        CreateMap<EmployeeExperience, EmployeeExperienceResponse>().ReverseMap();

        CreateMap<EmployeeContract, EmployeeContractDetailsDTO>().ReverseMap();
        CreateMap<EmployeeEducation, EmployeeEducationDetailsDTO>().ReverseMap();
        CreateMap<EmployeeExperience, EmployeeExperienceDetailsDTO>().ReverseMap();
    }
}
