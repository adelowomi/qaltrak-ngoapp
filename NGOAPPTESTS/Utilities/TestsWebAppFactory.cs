using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using TestContainers.Container.Abstractions.Hosting;
using TestContainers.Container.Database.Hosting;
using TestContainers.Container.Database.MySql;

namespace NGOAPPTESTS;

public class TestsWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{

    private readonly MySqlContainer _mySqlContainer = new ContainerBuilder<MySqlContainer>()
                .Build();

    public Task InitializeAsync()
    {
        return _mySqlContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<Context>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            var assembly = typeof(AppServiceExtensions).Assembly.GetName().Name;

            services.AddDbContext<Context>(options =>
            {
                options.UseMySql(_mySqlContainer.GetConnectionString(), MySqlServerVersion.AutoDetect(_mySqlContainer.GetConnectionString()), b => b.MigrationsAssembly(assembly)).UseCamelCaseNamingConvention();
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<Context>();

                db.Database.EnsureCreated();

                try
                {
                    new SeedData(db).SeedInitialData();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        });
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return _mySqlContainer.StopAsync();
    }
}
