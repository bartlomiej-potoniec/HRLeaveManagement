using AutoMapper;
using FluentAssertions;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.Features.LeaveType.Queries;
using HRLeaveManagement.Application.Features.LeaveType.QueryHandlers;
using HRLeaveManagement.Application.MappingProfiles;
using HRLeaveManagement.Application.Tests.Mocks;
using Moq;

namespace HRLeaveManagement.Application.Tests.Features.LeaveType.QueryHandlers;

public class GetAllLeaveTypesQueryHandlerTest
{
    private readonly Mock<ILeaveTypeRepository> _repositoryMock;
    private readonly Mock<IAppLogger<GetAllLeaveTypesQueryHandler>> _loggerMock;
    private readonly IMapper _mapper;

    public GetAllLeaveTypesQueryHandlerTest()
    {
        _repositoryMock = LeaveTypeRepositoryMock.GetLeaveTypeLeaveTypeRepositoryMock();
        _loggerMock = new Mock<IAppLogger<GetAllLeaveTypesQueryHandler>>();

        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<LeaveTypeProfile>();
        });

        _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task GetLeaveTypeListTest()
    {
        // Arrange
        var handler = new GetAllLeaveTypesQueryHandler(
            _repositoryMock.Object,
            _mapper,
            _loggerMock.Object
        );

        var query = new GetAllLeaveTypesQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().AllBeOfType<LeaveTypeDTO>();

        result.Count().Should().Be(3);
    }
}
