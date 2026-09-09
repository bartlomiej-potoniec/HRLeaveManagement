using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Domain.Document;
using HRLeaveManagement.Persistence.Department;
using HRLeaveManagement.Persistence.Department.Section;
using HRLeaveManagement.Persistence.Employee;
using HRLeaveManagement.Persistence.Document;
using HRLeaveManagement.Persistence.Leave.LeaveAllocation;
using HRLeaveManagement.Persistence.Leave.LeaveRequest;
using HRLeaveManagement.Persistence.Leave.LeaveType;
using HRLeaveManagement.Persistence.TimeTracking.RemoteWorkLimit;

namespace HRLeaveManagement.Persistence;

public static class PersistenceServiceRegistrationExtension
{
    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services,
                                                                 IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration
                .GetConnectionString("HrLeaveManagementConnectionString"))
        );

        services.AddScoped<IEmployeeDocumentNumberUniqueChecker, EmployeeDocumentNumberUniqueChecker>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();
        services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
        services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
        services.AddScoped<ILeaveAllocationRepository, LeaveAllocationRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ISectionRepository, SectionRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IRemoteWorkLimitRepository, RemoteWorkLimitRepository>();

        return services;
    }
}
