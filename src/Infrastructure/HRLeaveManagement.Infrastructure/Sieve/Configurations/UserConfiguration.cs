using HRLeaveManagement.Application.DTOs.Users;
using Sieve.Services;

namespace HRLeaveManagement.Infrastructure.Sieve.Configurations;

public class UserConfiguration : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper
            .Property<UserDetailsDTO>(u => u.FirstName)
            .CanFilter()
            .CanSort();

        mapper
            .Property<UserDetailsDTO>(u => u.LastName)
            .CanFilter()
            .CanSort();

        mapper
            .Property<UserDetailsDTO>(u => u.FullName)
            .CanFilter()
            .CanSort();

        mapper
            .Property<UserDetailsDTO>(u => u.UserName)
            .CanFilter()
            .CanSort();

        mapper
            .Property<UserDetailsDTO>(u => u.Email)
            .CanFilter()
            .CanSort();

        mapper
            .Property<UserDetailsDTO>(u => u.PhoneNumber)
            .CanFilter();

        mapper
            .Property<UserDetailsDTO>(u => u.AccountStatus)
            .CanFilter()
            .CanSort();
    }
}
