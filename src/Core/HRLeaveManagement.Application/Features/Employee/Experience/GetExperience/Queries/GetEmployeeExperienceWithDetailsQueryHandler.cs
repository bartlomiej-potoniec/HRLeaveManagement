using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.DTOs.Employees;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using AutoMapper;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Experience.GetExperience.Queries;

public sealed class GetEmployeeExperienceWithDetailsQueryHandler(IEmployeeRepository employeeRepository,
                                                                 IMapper mapper,
                                                                 IAppLogger<GetEmployeeExperienceWithDetailsQueryHandler> logger)
    : IRequestHandler<GetEmployeeExperienceWithDetailsQuery, EmployeeExperienceDetailsDTO>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetEmployeeExperienceWithDetailsQueryHandler> _logger = logger;

    public async Task<EmployeeExperienceDetailsDTO> Handle(GetEmployeeExperienceWithDetailsQuery request,
                                                           CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching employee contract with ID: {ExperienceId} for employee with ID: {EmployeeId} started", request.ExperienceId, request.EmployeeId);

        var employeeExperience = await _employeeRepository
            .GetExperienceByIdAsync(request.ExperienceId, cancellationToken)
            ?? throw new NotFoundException($"No contract with ID: {request.ExperienceId} found");

        var employeeExperienceDto = _mapper.Map<EmployeeExperienceDetailsDTO>(employeeExperience);

        _logger.LogInformation("Fetching employee contract with ID: {ExperienceId} for employee with ID: {EmployeeId} successful", request.ExperienceId, request.EmployeeId);

        return employeeExperienceDto;
    }
}
