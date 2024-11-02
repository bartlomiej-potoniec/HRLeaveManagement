using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Persistence.DbContexts;

namespace HRLeaveManagement.Persistence.Seeders;

public static class LeaveTypeSeeder
{
    public static void SeedLeaveTypes(this ApplicationDbContext dbContext)
    {
        if (dbContext.LeaveTypes.Any()) 
            return;

        LeaveType[] leaveTypes = [
            LeaveType.Create(name: "Urlop wypoczynkowy"),
            LeaveType.Create(name: "Urlop na żądanie"),
            LeaveType.Create(name: "Urlop okolicznościowy"),
            LeaveType.Create(name: "Urlop bezpłatny", paidFraction: 0.0M),
            LeaveType.Create(name: "Urlop macierzyński"),
            LeaveType.Create(name: "Urlop ojcowski"),
            LeaveType.Create(name: "Urlop rodzicielski"),
            LeaveType.Create(name: "Urlop wychowawczy", paidFraction: 0.0M),
            LeaveType.Create(name: "Opieka nad dzieckiem"),
            LeaveType.Create(name: "Urlop opiekuńczy", paidFraction: 0.0M),
            LeaveType.Create(name: "Urlop siła wyższa", paidFraction: 0.5M),
            LeaveType.Create(name: "Urlop na poszukiwanie pracy"),
            LeaveType.Create(name: "Urlop szkoleniowy"),
            LeaveType.Create(name: "Urlop z tytułu krwiodawstwa")
        ];

        dbContext.LeaveTypes.AddRange(leaveTypes);
        dbContext.SaveChanges();
    }
}
