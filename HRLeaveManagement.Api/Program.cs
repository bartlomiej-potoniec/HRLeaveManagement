using HRLeaveManagement.Application.Extensions;
using HRLeaveManagement.Infrastructure.Extensions;
using HRLeaveManagement.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterApplicationServices();
builder.Services.RegisterInfrastructureServices(builder.Configuration);
builder.Services.RegisterPersistenceServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("all", builder => builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors("all");

app.Run();
