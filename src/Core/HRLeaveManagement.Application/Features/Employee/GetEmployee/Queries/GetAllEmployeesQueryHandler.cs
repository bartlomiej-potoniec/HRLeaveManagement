using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.DTOs.Employees;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using AutoMapper;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.GetEmployee.Queries;

public sealed class GetAllEmployeesQueryHandler(IEmployeeRepository employeeRepository,
                                                IUserService userService,
                                                IServiceProvider serviceProvider,
                                                IMapper mapper,
                                                IAppLogger<GetAllEmployeesQueryHandler> logger)
    : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDTO>>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IUserService _userService = userService;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetAllEmployeesQueryHandler> _logger = logger;

    public async Task<IEnumerable<EmployeeDTO>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all employees started by user: {Username} with ID: {UserId}", _userService.UserName, _userService.UserId);

        var employees = await _employeeRepository.GetAllAsync(cancellationToken);
        var employeeDtos = new ConcurrentBag<EmployeeDTO>();

        await Parallel.ForEachAsync(employees, async (employee, token) =>
        {
            if (cancellationToken.IsCancellationRequested)
            { 
                return; 
            }

            using var scope = _serviceProvider.CreateScope();
            var scopedUserService = scope.ServiceProvider.GetRequiredService<IUserService>();

            var isUserEmployee = await scopedUserService.IsUserEmployeeByEmployeeIdAsync(employee.Id, cancellationToken);
            if (isUserEmployee is false)
            { 
                return; 
            }

            var user = await scopedUserService.GetUserByEmployeeIdAsync(employee.Id, cancellationToken);
            if (user is null)
            {
                return;
            }

            var userLeader = employee.LeaderId.HasValue 
                ? await scopedUserService.GetUserByEmployeeIdAsync(employee.LeaderId.Value, cancellationToken) 
                : null;

            var isLeader = await scopedUserService.IsUserInManagerRoleByEmployeeIdAsync(employee.Id, cancellationToken);

            var employeeDto = _mapper.Map<EmployeeDTO>(
                (user, employee),
                opt =>
                {
                    opt.Items["LeaderName"] = userLeader is not null
                        ? $"{userLeader.FirstName} {userLeader.LastName}"
                        : null;

                    opt.Items["IsLeader"] = isLeader;
                } 
            );

            employeeDtos.Add(employeeDto);
        });

        _logger.LogInformation("Fetching all employees by user: {Username} with ID: {UserId} successful", _userService.UserName, _userService.UserId);

        return [.. employeeDtos];
    }
}
