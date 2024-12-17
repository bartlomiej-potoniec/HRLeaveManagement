using HRLeaveManagement.Domain.Entities;
using AutoMapper;
using HRLeaveManagement.Application.DTOs.User;

namespace HRLeaveManagement.Application.MappingResolvers;

public class IsCurrentlyEmployedResolver : IValueResolver<(UserDTO userDto, Employee employee), object, bool>
{
    public bool Resolve((UserDTO userDto, Employee employee) source,
                        object destination,
                        bool destMember,
                        ResolutionContext context)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var isCurrentlyEmployeed = false;

        source.employee.EmploymentContracts.ForEach(contract =>
        {
            var employeedFrom = contract.StartedAt;
            var employeedTo = contract.ExpiredAt;

            if (today > employeedFrom && employeedTo is null)
            {
                isCurrentlyEmployeed = true;
                return;
            }

            if (today > employeedFrom && today < employeedTo)
            {
                isCurrentlyEmployeed = true;
                return;
            }
        });

        return isCurrentlyEmployeed;
    }
}
