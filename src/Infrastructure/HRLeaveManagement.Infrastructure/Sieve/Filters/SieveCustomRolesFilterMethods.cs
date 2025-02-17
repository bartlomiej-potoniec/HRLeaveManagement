using HRLeaveManagement.Application.DTOs.Users;
using Sieve.Services;

namespace HRLeaveManagement.Infrastructure.Sieve.Filters;

public class SieveCustomRolesFilterMethods : ISieveCustomFilterMethods
{
    public IQueryable<UserDetailsDTO> Roles(IQueryable<UserDetailsDTO> source, string op, string[] values)
    {
        if (values is null || !values.Any()) 
            return source;

        var result = source.Where(
            p => p.Roles != null && 
            p.Roles.Any(role => values.Contains(role, StringComparer.OrdinalIgnoreCase))
        );

        return result;
    }

    public IQueryable<UserDetailsDTO> EmployeeId(IQueryable<UserDetailsDTO> source, string op, string[] values)
    {
        if (values is null || !values.Any())
            return source;

        var result = source.Where(s => s.EmployeeId == null);

        return result;
    }
}
