using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.DTOs.Sections;
using HRLeaveManagement.Application.Features.Department.Queries;
using MediatR;
using AutoMapper;

namespace HRLeaveManagement.Application.Features.Department.QueryHandlers;

public sealed class GetAllSectionsByDepartmentIdQueryHandler(ISectionRepository sectionRepository,
                                                             IMapper mapper,
                                                             IAppLogger<GetAllSectionsByDepartmentIdQueryHandler> logger)
    : IRequestHandler<GetAllSectionsByDepartmentIdQuery, IEnumerable<SectionDTO>>
{
    private readonly ISectionRepository _sectionRepository = sectionRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetAllSectionsByDepartmentIdQueryHandler> _logger = logger;

    public async Task<IEnumerable<SectionDTO>> Handle(GetAllSectionsByDepartmentIdQuery request,
                                                      CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all sections for department with ID: {Id} started", request.DepartmentId);

        var sections = await _sectionRepository.GetAllByDepartmentIdAsync(request.DepartmentId, cancellationToken);
        var sectionDtos = _mapper.Map<IEnumerable<SectionDTO>>(sections);

        _logger.LogInformation("Fetching all sections for department with ID: {Id} successful", request.DepartmentId);

        return sectionDtos;
    }
}
