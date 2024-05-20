using AutoMapper;
using Hangfire;
using NGOAPP;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCoreServices(builder.Configuration, builder.Host);
builder.Services.AddAppServices(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapIdentityEndpoints();


app.UseCors(x => x
.AllowAnyMethod()
.AllowAnyHeader()
.AllowAnyOrigin());

app.UseRouting();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Context>();
    // use context
    new SeedData(context).SeedInitialData();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHangfireDashboard("/fire");
app.UseHangfireServer();

app.UseHttpsRedirection();

app.MapControllers();

IQueryableExtensions.Configure(app.Services.GetRequiredService<IMapper>(), app.Services.GetRequiredService<AutoMapper.IConfigurationProvider>());

app.Run();

