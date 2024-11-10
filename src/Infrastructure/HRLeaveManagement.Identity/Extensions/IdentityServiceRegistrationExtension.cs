using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Identity.Options;
using HRLeaveManagement.Identity.DbContexts;
using HRLeaveManagement.Identity.Models;
using HRLeaveManagement.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HRLeaveManagement.Identity.Extensions;

public static class IdentityServiceRegistrationExtension
{
    public static IServiceCollection RegisterIdentityServices(this IServiceCollection services,
                                                              IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("JwtSettings"));
        services.Configure<CredentialOptions>(configuration.GetSection("CredentialOptions"));

        services.AddDbContext<ApplicationIdentityDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("HrLeaveManagementConnectionString")
            )
        );

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
            .AddDefaultTokenProviders();

        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ICredentialService, CredentialService>();

        services
            .AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => 
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!))
                };
            });

        return services;
    }
}
