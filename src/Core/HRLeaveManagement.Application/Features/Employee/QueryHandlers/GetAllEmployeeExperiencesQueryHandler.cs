using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Features.Employee.Queries;
using HRLeaveManagement.Application.DTOs.Employees;
using AutoMapper;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.QueryHandlers;

public sealed class GetAllEmployeeExperiencesQueryHandler(IEmployeeRepository employeeRepository,
                                                          IMapper mapper,
                                                          IAppLogger<GetAllEmployeeExperiencesQueryHandler> logger)
    : IRequestHandler<GetAllEmployeeExperiencesQuery, IEnumerable<EmployeeExperienceDetailsDTO>>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetAllEmployeeExperiencesQueryHandler> _logger = logger;

    public async Task<IEnumerable<EmployeeExperienceDetailsDTO>> Handle(GetAllEmployeeExperiencesQuery request,
                                                                        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all employee experiences for employee with ID: {Id} started", request.EmployeeId);

        var employeeExperiences = await _employeeRepository.GetAllExperiencesByEmployeeIdAsync(request.EmployeeId, cancellationToken);
        var employeeExperienceDtos = _mapper.Map<IEnumerable<EmployeeExperienceDetailsDTO>>(employeeExperiences);

        _logger.LogInformation("Fetching all employee experiences for employee with ID: {Id} successful", request.EmployeeId);

        return employeeExperienceDtos;
    }
}
