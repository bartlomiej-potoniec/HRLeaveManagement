using DomainLeaveType = HRLeaveManagement.Domain.Leave.LeaveType.LeaveType;

namespace HRLeaveManagement.Persistence.Leave.LeaveType;

public static class LeaveTypeSeeder
{
    public static async Task SeedLeaveTypesAsync(this ApplicationDbContext dbContext)
    {
        if (dbContext.LeaveTypes.Any())
        {
            return;
        }

        var emptyRule = EmptyLeaveTypeNameUniqueChecker.Instance;

        DomainLeaveType[] leaveTypes = [
            await DomainLeaveType.CreateAsync(emptyRule, "Urlop wypoczynkowy"),
            await DomainLeaveType.CreateAsync(emptyRule, "Urlop na żądanie"),
            await DomainLeaveType.CreateAsync(emptyRule, "Urlop okolicznościowy"),
            await DomainLeaveType.CreateAsync(emptyRule, "Urlop bezpłatny", paidFraction : 0.0M),
            await DomainLeaveType.CreateAsync(emptyRule, "Urlop macierzyński"),
            await DomainLeaveType.CreateAsync(emptyRule, "Urlop ojcowski"),
            await DomainLeaveType.CreateAsync(emptyRule, "Urlop rodzicielski"),
            await DomainLeaveType.CreateAsync(emptyRule, "Urlop wychowawczy", paidFraction : 0.0M),
            await DomainLeaveType.CreateAsync(emptyRule, "Opieka nad dzieckiem"),
            await DomainLeaveType.CreateAsync(emptyRule, "Urlop opiekuńczy", paidFraction : 0.0M),
            await DomainLeaveType.CreateAsync(emptyRule, "Urlop siła wyższa", paidFraction : 0.5M),
            await DomainLeaveType.CreateAsync(emptyRule, "Urlop na poszukiwanie pracy"),
            await DomainLeaveType.CreateAsync(emptyRule, "Urlop szkoleniowy"),
            await DomainLeaveType.CreateAsync(emptyRule, "Urlop z tytułu krwiodawstwa")
        ];

        await dbContext.LeaveTypes.AddRangeAsync(leaveTypes);
        await dbContext.SaveChangesAsync();
    }
}
