using AutoMapper;
using HRLeaveManagement.Application.DTOs.RemoteWorkLimits;
using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.MappingProfiles;

public class RemoteWorkLimitProfile : Profile
{
    public RemoteWorkLimitProfile()
    {
        CreateMap<RemoteWorkLimit, RemoteWorkLimitDTO>().ReverseMap();
        CreateMap<RemoteWorkLimit, RemoteWorkLimitDetailsDTO>().ReverseMap();
    }
}
