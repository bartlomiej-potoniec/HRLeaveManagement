using Sieve.Models;

namespace HRLeaveManagement.Infrastructure.Sieve.Mappers;

public static class SieveModelMapper
{
    public static SieveModel Map(int? pageSize = null,
                                 int? pageNumber = null,
                                 string? sorts = null,
                                 string? filters = null)
        => new()
        {
            PageSize = pageSize,
            Page = pageNumber,
            Sorts = sorts,
            Filters = filters
        };
}
