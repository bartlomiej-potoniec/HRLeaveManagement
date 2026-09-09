using AutoMapper;
using HRLeaveManagement.Application.DTOs.RemoteWorkLimits;
using HRLeaveManagement.Domain.TimeTracking.RemoteWorkLimit;

namespace HRLeaveManagement.Application.MappingProfiles;

public class RemoteWorkLimitProfile : Profile
{
    public RemoteWorkLimitProfile()
    {
        CreateMap<RemoteWorkLimit, RemoteWorkLimitDTO>().ReverseMap();
        CreateMap<RemoteWorkLimit, RemoteWorkLimitDetailsDTO>().ReverseMap();
    }
}
