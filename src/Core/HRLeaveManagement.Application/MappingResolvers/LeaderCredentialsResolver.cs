using HRLeaveManagement.Application.DTOs.Employees;
using HRLeaveManagement.Domain.Entities;
using AutoMapper;
using HRLeaveManagement.Application.DTOs.User;

namespace HRLeaveManagement.Application.MappingResolvers;

public class LeaderCredentialsResolver 
    : IValueResolver<(UserDTO userDto, Employee employee, IEnumerable<UserDTO> leaders), object, string?>
{
    public string? Resolve((UserDTO userDto, Employee employee, IEnumerable<UserDTO> leaders) source,
                          object destination,
                          string? destMember,
                          ResolutionContext context)
        =>
            source.leaders.FirstOrDefault(l => l.EmployeeId == source.employee.LeaderId) is UserDTO leaderUser
                ? $"{ leaderUser.FirstName } { leaderUser.LastName }"
                : null;
}
