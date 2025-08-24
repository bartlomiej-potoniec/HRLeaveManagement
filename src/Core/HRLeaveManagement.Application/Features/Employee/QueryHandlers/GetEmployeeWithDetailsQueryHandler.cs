using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.DTOs.Employees;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.Employee.Queries;
using MediatR;
using AutoMapper;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;

namespace HRLeaveManagement.Application.Features.Employee.QueryHandlers;

public sealed class GetEmployeeWithDetailsQueryHandler(IEmployeeRepository repository,
                                                       IUserService userService,
                                                       IMapper mapper,
                                                       IAppLogger<GetEmployeeWithDetailsQueryHandler> logger)
    : IRequestHandler<GetEmployeeWithDetailsQuery, EmployeeDetailsDTO>
{
    private readonly IEmployeeRepository _repository = repository;
    private readonly IUserService _userService = userService;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetEmployeeWithDetailsQueryHandler> _logger = logger;

    public async Task<EmployeeDetailsDTO> Handle(GetEmployeeWithDetailsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching employee with ID: {EmployeeId} started by user: {Username} with ID: {UserId}", request.Id, _userService.UserName, _userService.UserId);

        var user = await _userService.GetUserByEmployeeIdAsync(request.Id, cancellationToken);
        var usersInRoleManager = await _userService.GetAllUsersInRoleAsync("Manager", cancellationToken);
       
        var employee = await _repository
            .GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"No employee for ID: { request.Id } found");

        var leaderUser = usersInRoleManager.FirstOrDefault(l => l.EmployeeId == employee.LeaderId);

        var employeeDetailsDto = _mapper.Map<EmployeeDetailsDTO>(
            (user, employee),
            opt => opt.Items["LeaderName"] = leaderUser is not null
                ? $"{ leaderUser.FirstName } { leaderUser.LastName }"
                : null
        );

        _logger.LogInformation("Fetching employee with ID: {EmployeeId} by user: {Username} with ID: {UserId} successful", request.Id, _userService.UserName, _userService.UserId);

        return employeeDetailsDto;
    }
}
