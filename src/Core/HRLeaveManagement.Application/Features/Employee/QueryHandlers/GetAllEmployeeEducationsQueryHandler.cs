using AutoMapper;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.DTOs.Employees;
using HRLeaveManagement.Application.Features.Employee.Queries;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.QueryHandlers;

public sealed class GetAllEmployeeEducationsQueryHandler(IEmployeeRepository employeeRepository,
                                                         IMapper mapper,
                                                         IAppLogger<GetAllEmployeeEducationsQueryHandler> logger)
    : IRequestHandler<GetAllEmployeeEducationsQuery, IEnumerable<EmployeeEducationDetailsDTO>>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetAllEmployeeEducationsQueryHandler> _logger = logger;

    public async Task<IEnumerable<EmployeeEducationDetailsDTO>> Handle(GetAllEmployeeEducationsQuery request,
                                                                       CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all employee educations for employee with ID: {Id} started", request.EmployeeId);

        var employeeEducations = await _employeeRepository.GetAllEducationsByEmployeeIdAsync(request.EmployeeId, cancellationToken);
        var employeeEducationDtos = _mapper.Map<IEnumerable<EmployeeEducationDetailsDTO>>(employeeEducations);

        _logger.LogInformation("Fetching all employee educations for employee with ID: {Id} successful", request.EmployeeId);

        return employeeEducationDtos;
    }
}
