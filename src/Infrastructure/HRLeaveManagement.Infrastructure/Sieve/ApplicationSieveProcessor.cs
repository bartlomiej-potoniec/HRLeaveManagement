using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace HRLeaveManagement.Infrastructure.Sieve;

public sealed class ApplicationSieveProcessor(IOptions<SieveOptions> options,
                                              ISieveCustomFilterMethods customFilterMethods) 
    : SieveProcessor(options, customFilterMethods)
{
    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        mapper.ApplyConfigurationsFromAssembly(typeof(ApplicationSieveProcessor).Assembly);

        return base.MapProperties(mapper);
    }
}
