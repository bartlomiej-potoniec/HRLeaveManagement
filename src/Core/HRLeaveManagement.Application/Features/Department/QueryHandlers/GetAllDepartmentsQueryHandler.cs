using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Features.Department.Queries;
using HRLeaveManagement.Application.DTOs.Departments;
using MediatR;
using AutoMapper;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;

namespace HRLeaveManagement.Application.Features.Department.QueryHandlers;

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

        var departments = await _departmentRepository.GetAllAsync(cancellationToken);
        var departmentsDto = _mapper.Map<IEnumerable<DepartmentDTO>>(departments);

        _logger.LogInformation("Fetching all departments successful");

        return departmentsDto;
    }
}
