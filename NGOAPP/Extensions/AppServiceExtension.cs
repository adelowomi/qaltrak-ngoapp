using NGOAPP.Services;
using Microsoft.EntityFrameworkCore;

namespace NGOAPP;

public static class AppServiceExtensions
{
    public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddTransient<ICodeService, CodeService>();
        services.AddTransient<IPostmarkHelper, PostmarkHelper>();
    }


    public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration, IHostBuilder host)
    {
        var _appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
        var appSettingsSection = configuration.GetSection(nameof(AppSettings));
        services.Configure<AppSettings>(appSettingsSection);
        var assembly = typeof(AppServiceExtensions).Assembly.GetName().Name;
        services.AddDbContext<Context>(options =>
            {
                options.UseMySql(_appSettings.ConnectionString, MySqlServerVersion.AutoDetect(_appSettings.ConnectionString), b => b.MigrationsAssembly(assembly)).UseCamelCaseNamingConvention();
                options.UseOpenIddict<int>();
            });

        // add utilities to the service container

        // inject appsettings  
        services.AddCustomUserIdentity(configuration);
        services.AddAppServices(configuration);

        // Add services to the container.

        services.AddAuthorization();
        services.AddAuthentication();

        services.AddCors();

        // add controllers
        services.AddControllers();

        // add auto mapper
        services.AddAutoMapper(typeof(Program));
        services.AddSwaggerGen(x =>
        {
            x.AddSwaggerAuthOptions();
            x.OperationFilter<SwaggerHeaderFilter>();
        });
    }
}
