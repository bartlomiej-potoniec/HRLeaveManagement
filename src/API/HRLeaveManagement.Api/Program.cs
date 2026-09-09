using HRLeaveManagement.Api.Authorization.Handlers;
using HRLeaveManagement.Api.Authorization.Requirements;
using HRLeaveManagement.Api.Middleware;
using HRLeaveManagement.Application;
using HRLeaveManagement.Identity.Extensions;
using HRLeaveManagement.Infrastructure.Extensions;
using HRLeaveManagement.Persistence;
using HRLeaveManagement.Persistence.Leave.LeaveType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog((context, loggerConfig) 
    => loggerConfig
        .WriteTo.Console()
        .ReadFrom.Configuration(context.Configuration));

builder.Services.RegisterApplicationServices();
builder.Services.RegisterInfrastructureServices(builder.Configuration);
builder.Services.RegisterPersistenceServices(builder.Configuration);
builder.Services.RegisterIdentityServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("all", builder => builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IAuthorizationHandler, IsEmployeeRequirementHandler>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("IsEmployee", policy => 
        policy.Requirements.Add(new IsEmployeeRequirement()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Migrate pednind migrations & seed data
using var scope = app.Services.CreateScope();

var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
var pendingMigrations = dbContext.Database.GetPendingMigrations();

if (pendingMigrations.Any())
{
    await dbContext.Database.MigrateAsync();
}

await dbContext.SeedLeaveTypesAsync();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("all");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
