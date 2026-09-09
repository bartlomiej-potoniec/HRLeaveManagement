using HRLeaveManagement.Application.DTOs.LeaveAllocations;
using AutoMapper;
using HRLeaveManagement.Domain.Leave.LeaveAllocation;

namespace HRLeaveManagement.Application.MappingProfiles;

public class LeaveAllocationProfile : Profile
{
    public LeaveAllocationProfile()
    {
        CreateMap<LeaveAllocation, LeaveAllocationDTO>()
            .ForMember(dest => dest.LeaveTypeName, opt => opt.MapFrom(src => src.LeaveType.Name))
            .ForMember(dest => dest.EmployeeName, opt =>
                opt.MapFrom((src, dest, destMember, context) => 
                    context.Items.TryGetValue("EmployeeName", out object? value) ? value as string : null
                )
            )
            .ReverseMap();
        
        CreateMap<LeaveAllocation, LeaveAllocationDetailsDTO>().ReverseMap();
    }
}
