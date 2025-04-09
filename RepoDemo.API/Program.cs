using RepoDemo.GenericRepository.Repository;
using RepoDemo.Service;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using RepoDemo.Data;
using RepoDemo.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Register services
builder.Services.AddServiceConfiguration();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddSingleton<TelemetryClient>(sp =>
{
    var options = sp.GetRequiredService<IOptions<TelemetryConfiguration>>();
    return new TelemetryClient(options.Value);
});

// Register TelemetryClientWrapper as the implementation of ITelemetryClient
builder.Services.AddSingleton<ITelemetryClient, TelemetryClientWrapper>(sp =>
{
    var telemetryClient = sp.GetRequiredService<TelemetryClient>();
    return new TelemetryClientWrapper(telemetryClient);
});

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    ServiceConfiguration.SetUpDemoData(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RepoDemo API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
