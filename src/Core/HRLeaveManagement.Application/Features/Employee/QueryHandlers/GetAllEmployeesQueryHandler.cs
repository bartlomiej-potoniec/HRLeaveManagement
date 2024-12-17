using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Features.Employee.Queries;
using HRLeaveManagement.Application.DTOs.Employees;
using MediatR;
using AutoMapper;
using HRLeaveManagement.Application.Exceptions;

namespace HRLeaveManagement.Application.Features.Employee.QueryHandlers;

public sealed class GetAllEmployeesQueryHandler(IEmployeeRepository employeeRepository,
                                                IUserService userService,
                                                IMapper mapper,
                                                IAppLogger<GetAllEmployeesQueryHandler> logger)
    : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDTO>>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IUserService _userService = userService;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetAllEmployeesQueryHandler> _logger = logger;

    public async Task<IEnumerable<EmployeeDTO>> Handle(GetAllEmployeesQuery request,
                                                       CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all employees started by user: {Username} with ID: {UserId}", _userService.UserName, _userService.UserId);

        var usersInRoleEmployee = await _userService.GetAllUsersInRole("Employee");
        var usersInRoleManager = await _userService.GetAllUsersInRole("Manager");

        var usersWithCompletedEmployeeInfo = usersInRoleEmployee
            .Where(e => e.EmployeeId is not null)
            .ToList();

        var employees = await _employeeRepository.GetAllAsync();

        var employeeDtos = employees
            .Select(employee =>
            {
                var user = usersWithCompletedEmployeeInfo
                    .FirstOrDefault(u => u.EmployeeId == employee.Id)
                    ?? throw new NotFoundException($"No user with employee ID: {employee.Id} found");

                var leaderUser = usersInRoleManager.FirstOrDefault(l => l.EmployeeId == employee.LeaderId);

                var employeeDto = _mapper.Map<EmployeeDTO>(
                    (user, employee),
                    opt => opt.Items["LeaderName"] = leaderUser is not null 
                        ? $"{ leaderUser.FirstName } { leaderUser.FirstName }" 
                        : null
                );

                return employeeDto;
            })
            .ToList();

        _logger.LogInformation("Fetching all employees by user: {Username} with ID: {UserId} successful", _userService.UserName, _userService.UserId);

        return employeeDtos;
    }
}
