using Asp.Versioning.ApiExplorer;
using Booking.API.Endpoints;
using Booking.API.Extensions;
using Booking.API.OpenAPI;
using Booking.Application;
using Booking.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigSwaggerOptions>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (ApiVersionDescription description in app.DescribeApiVersions())
        {
            string url = $"/swagger/{description.GroupName}/swagger.json";
            string name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
    app.ApplyMigrations();
    //  app.SeedData();
}

app.UseHttpsRedirection();
app.UseRequestContextLogging();
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.UseCustomExceptionHandler();
app.MapControllers();

// using method 1
app.MapBookingEndpoint();

// method 2: auto map api versioning for endpoint
//ApiVersionSet apiVersionSet = app.NewApiVersionSet()
//               .HasApiVersion(new ApiVersion(1))
//               .HasApiVersion(new ApiVersion(2))
//               .ReportApiVersions()
//               .Build();

//var routeGroupBuilder = app.MapGroup("api/v{version:apiVersion}").WithApiVersionSet(apiVersionSet);
//routeGroupBuilder.MapBookingEndpoint();
// end

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
