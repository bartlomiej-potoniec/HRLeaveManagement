using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.DTOs.Employees;
using HRLeaveManagement.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Education.GetEducation.Queries;

public sealed class GetEmployeeEducationWithDetailsQueryHandler(IEmployeeRepository employeeRepository,
                                                                IMapper mapper,
                                                                IAppLogger<GetEmployeeEducationWithDetailsQueryHandler> logger)
    : IRequestHandler<GetEmployeeEducationWithDetailsQuery, EmployeeEducationDetailsDTO>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetEmployeeEducationWithDetailsQueryHandler> _logger = logger;

    public async Task<EmployeeEducationDetailsDTO> Handle(GetEmployeeEducationWithDetailsQuery request,
                                                          CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching employee education with ID: {EducationId} for employee with ID: {EmployeeId} started", request.EducationId, request.EmployeeId);

        var employeeEducation = await _employeeRepository
            .GetEducationByIdAsync(request.EducationId, cancellationToken)
            ?? throw new NotFoundException($"No contract with ID: { request.EducationId } found");

        var employeeEducationDto = _mapper.Map<EmployeeEducationDetailsDTO>(employeeEducation);

        _logger.LogInformation("Fetching employee education with ID: {EducationId} for employee with ID: {EmployeeId} successful", request.EducationId, request.EmployeeId);

        return employeeEducationDto;
    }
}
