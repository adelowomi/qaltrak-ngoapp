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

app.Run();

