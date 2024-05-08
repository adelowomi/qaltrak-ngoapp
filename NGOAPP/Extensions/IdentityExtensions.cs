using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NGOAPP.Models.IdentityModels;
using NGOAPP.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NGOAPP;

public static class IdentityExtensions
{
    public static void AddCustomUserIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityApiEndpoints<User>(AddIdentityOptions)
        .AddRoles<Role>()
        .AddRoleManager<RoleManager<Role>>()
        .AddEntityFrameworkStores<Context>();



        services.AddOptions<IdentityConfig>()
            .Bind(configuration.GetSection(nameof(IdentityConfig)))
            .ValidateDataAnnotations();

        services.AddSingleton<IEmailSender<User>, IdentityEmailSender>();
    }

    public static void MapIdentityEndpoints(this IEndpointRouteBuilder endpointBuilder)
    {
        var routeGroup = endpointBuilder.MapGroup(Routes.Identity)
                                        .WithTags(nameof(Routes.Identity))
                                        .MapIdentityApi<User>();
    }

    public static void AddIdentityOptions(IdentityOptions options)
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 1;

        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = true;
    }

    public static void AddSwaggerAuthOptions(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                new List<string>()
            }
        });
    }

}
