using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.Departments.Queries;
using HRLeaveManagement.Application.Exceptions;
using MediatR;
using AutoMapper;
using HRLeaveManagement.Application.DTOs.Departments;

namespace HRLeaveManagement.Application.Features.Departments.QueryHandlers;

public sealed class GetDepartmentWithDetailsQueryHandler(IDepartmentRepository departmentRepository,
                                                         IMapper mapper,
                                                         IAppLogger<GetDepartmentWithDetailsQueryHandler> logger)
    : IRequestHandler<GetDepartmentWithDetailsQuery, DepartmentDetailsDTO>
{
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetDepartmentWithDetailsQueryHandler> _logger = logger;

    public async Task<DepartmentDetailsDTO> Handle(GetDepartmentWithDetailsQuery request,
                                                   CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching department with ID: {Id} started", request.DepartmentId);

        var department = await _departmentRepository
            .GetByIdAsync(request.DepartmentId)
            ?? throw new NotFoundException($"No department with ID: {request.DepartmentId} found");

        var departmentDto = _mapper.Map<DepartmentDetailsDTO>(department);

        _logger.LogInformation("Fetching department with ID: {Id} successful", request.DepartmentId);

        return departmentDto;
    }
}
