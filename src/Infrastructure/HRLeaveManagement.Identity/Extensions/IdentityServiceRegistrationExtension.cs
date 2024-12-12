using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Identity.Options;
using HRLeaveManagement.Identity.DbContexts;
using HRLeaveManagement.Identity.Models;
using HRLeaveManagement.Identity.Services;
using HRLeaveManagement.Identity.Adapters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Text;

namespace HRLeaveManagement.Identity.Extensions;

public static class IdentityServiceRegistrationExtension
{
    public static IServiceCollection RegisterIdentityServices(this IServiceCollection services,
                                                              IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        services.Configure<CredentialOptions>(configuration.GetSection(nameof(CredentialOptions)));

        services.AddDbContext<ApplicationIdentityDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("HrLeaveManagementConnectionString")
            )
        );

        services
            .AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
            .AddDefaultTokenProviders();

        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ICredentialService, CredentialService>();
        services.AddTransient<IJwtService, JwtService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IIdentityResult, IdentityResultAdapter>();

        services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
        services.AddHttpContextAccessor();

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
                    ValidIssuer = configuration["JwtOptions:Issuer"],
                    ValidAudience = configuration["JwtOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:Key"]!))
                };
            });

        return services;
    }
}
