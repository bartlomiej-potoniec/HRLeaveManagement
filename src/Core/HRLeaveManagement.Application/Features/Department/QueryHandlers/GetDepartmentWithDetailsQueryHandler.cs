using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.Department.Queries;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.DTOs.Departments;
using MediatR;
using AutoMapper;

namespace HRLeaveManagement.Application.Features.Department.QueryHandlers;

public sealed class GetDepartmentWithDetailsQueryHandler(IDepartmentRepository departmentRepository,
                                                         IMapper mapper,
                                                         IAppLogger<GetDepartmentWithDetailsQueryHandler> logger)
    : IRequestHandler<GetDepartmentWithDetailsQuery, DepartmentDetailsDTO>
{
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetDepartmentWithDetailsQueryHandler> _logger = logger;

    public async Task<DepartmentDetailsDTO> Handle(GetDepartmentWithDetailsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching department with ID: {Id} started", request.Id);

        var department = await _departmentRepository
            .GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"No department with ID: { request.Id } found");

        var departmentDto = _mapper.Map<DepartmentDetailsDTO>(department);

        _logger.LogInformation("Fetching department with ID: {Id} successful", request.Id);

        return departmentDto;
    }
}
