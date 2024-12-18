using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.Departments.Queries;
using HRLeaveManagement.Application.DTOs.Departments;
using AutoMapper;
using MediatR;

namespace HRLeaveManagement.Application.Features.Departments.QueryHandlers;

public sealed class GetAllDepartmentsQueryHandler(IDepartmentRepository departmentRepository,
                                                  IMapper mapper,
                                                  IAppLogger<GetAllDepartmentsQueryHandler> logger)
    : IRequestHandler<GetAllDepartmentsQuery, IEnumerable<DepartmentDTO>>
{
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetAllDepartmentsQueryHandler> _logger = logger;

    public async Task<IEnumerable<DepartmentDTO>> Handle(GetAllDepartmentsQuery request,
                                                         CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all departments started");

        var departments = await _departmentRepository.GetAllAsync();
        var departmentsDto = _mapper.Map<IEnumerable<DepartmentDTO>>(departments);

        _logger.LogInformation("Fetching all departments successful");

        return departmentsDto;
    }
}
