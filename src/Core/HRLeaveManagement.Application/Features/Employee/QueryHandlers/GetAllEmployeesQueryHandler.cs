using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Features.Employee.Queries;
using HRLeaveManagement.Application.DTOs.Employees;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using MediatR;
using AutoMapper;

namespace HRLeaveManagement.Application.Features.Employee.QueryHandlers;

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

    public async Task<IEnumerable<EmployeeDTO>> Handle(GetAllEmployeesQuery request,
                                                       CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all employees started by user: {Username} with ID: {UserId}", _userService.UserName, _userService.UserId);

        var employees = await _employeeRepository.GetAllAsync();
        var employeesDtos = new ConcurrentBag<EmployeeDTO>();

        await Parallel.ForEachAsync(employees, async (employee, token) =>
        {
            using var scope = _serviceProvider.CreateScope();
            var scopedUserService = scope.ServiceProvider.GetRequiredService<IUserService>();

            var isUserEmployee = await scopedUserService.IsUserEmployeeByEmployeeId(employee.Id);
            if (isUserEmployee is false) return;

            var user = await scopedUserService.GetUserByEmployeeId(employee.Id);
            if (user is null) return;

            var userLeader = employee.LeaderId.HasValue 
                ? await scopedUserService.GetUserByEmployeeId(employee.LeaderId.Value) 
                : null;

            var isLeader = await scopedUserService.IsUserInManagerRoleByEmployeeId(employee.Id);

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

            employeesDtos.Add(employeeDto);
        });

        _logger.LogInformation("Fetching all employees by user: {Username} with ID: {UserId} successful", _userService.UserName, _userService.UserId);

        return [.. employeesDtos];
    }
}
