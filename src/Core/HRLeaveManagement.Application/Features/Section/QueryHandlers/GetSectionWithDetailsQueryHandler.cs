using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.DTOs.Sections;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.Section.Queries;
using MediatR;
using AutoMapper;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;

namespace HRLeaveManagement.Application.Features.Section.QueryHandlers;

public sealed class GetSectionWithDetailsQueryHandler(ISectionRepository sectionRepository,
                                                      IMapper mapper,
                                                      IAppLogger<GetSectionWithDetailsQueryHandler> logger)
    : IRequestHandler<GetSectionWithDetailsQuery, SectionDetailsDTO>
{
    private readonly ISectionRepository _sectionRepository = sectionRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetSectionWithDetailsQueryHandler> _logger = logger;

    public async Task<SectionDetailsDTO> Handle(GetSectionWithDetailsQuery request,
                                                CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching section with ID: {Id} started", request.Id);

        var section = await _sectionRepository
            .GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"No section with ID: { request.Id } found");

        var sectionDto = _mapper.Map<SectionDetailsDTO>(section);

        _logger.LogInformation("Fetching section with ID: {Id} successful", request.Id);

        return sectionDto;
    }
}
