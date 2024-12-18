using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.Sections.Queries;
using HRLeaveManagement.Application.DTOs.Sections;
using MediatR;
using AutoMapper;

namespace HRLeaveManagement.Application.Features.Sections.QueryHandlers;

public sealed class GetAllSectionsQueryHandler(ISectionRepository sectionRepository,
                                               IMapper mapper,
                                               IAppLogger<GetAllSectionsQueryHandler> logger) 
    : IRequestHandler<GetAllSectionsQuery, IEnumerable<SectionDTO>>
{
    private readonly ISectionRepository _sectionRepository = sectionRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetAllSectionsQueryHandler> _logger = logger;

    public async Task<IEnumerable<SectionDTO>> Handle(GetAllSectionsQuery request,
                                                      CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all sections started");

        var sections = await _sectionRepository.GetAllAsync();
        var sectionDtos = _mapper.Map<IEnumerable<SectionDTO>>(sections);

        _logger.LogInformation("Fetching all sections successful");

        return sectionDtos;
    }
}
