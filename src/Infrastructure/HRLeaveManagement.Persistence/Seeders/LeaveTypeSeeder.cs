using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Persistence.DbContexts;
using HRLeaveManagement.Persistence.RuleSets;

namespace HRLeaveManagement.Persistence.Seeders;

public static class LeaveTypeSeeder
{
    public static async Task SeedLeaveTypesAsync(this ApplicationDbContext dbContext)
    {
        if (dbContext.LeaveTypes.Any())
        {
            return;
        }

        var emptyRule = EmptyLeaveTypeRuleSet.Instance;

        LeaveType[] leaveTypes = [
            await LeaveType.CreateAsync(emptyRule, "Urlop wypoczynkowy"),
            await LeaveType.CreateAsync(emptyRule, "Urlop na żądanie"),
            await LeaveType.CreateAsync(emptyRule, "Urlop okolicznościowy"),
            await LeaveType.CreateAsync(emptyRule, "Urlop bezpłatny", paidFraction : 0.0M),
            await LeaveType.CreateAsync(emptyRule, "Urlop macierzyński"),
            await LeaveType.CreateAsync(emptyRule, "Urlop ojcowski"),
            await LeaveType.CreateAsync(emptyRule, "Urlop rodzicielski"),
            await LeaveType.CreateAsync(emptyRule, "Urlop wychowawczy", paidFraction : 0.0M),
            await LeaveType.CreateAsync(emptyRule, "Opieka nad dzieckiem"),
            await LeaveType.CreateAsync(emptyRule, "Urlop opiekuńczy", paidFraction : 0.0M),
            await LeaveType.CreateAsync(emptyRule, "Urlop siła wyższa", paidFraction : 0.5M),
            await LeaveType.CreateAsync(emptyRule, "Urlop na poszukiwanie pracy"),
            await LeaveType.CreateAsync(emptyRule, "Urlop szkoleniowy"),
            await LeaveType.CreateAsync(emptyRule, "Urlop z tytułu krwiodawstwa")
        ];

        await dbContext.LeaveTypes.AddRangeAsync(leaveTypes);
        await dbContext.SaveChangesAsync();
    }
}
