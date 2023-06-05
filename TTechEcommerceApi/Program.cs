using EcommerceApi.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog;
using TTechEcommerceApi.Extensions;
using TTechEcommerceApi.Filters.ActionFilters;
using TTechEcommerceApi.Authentication;
using TTechEcommerceApi.Interface;
using TTechEcommerceApi.Middlewares;
using TTechEcommerceApi.Repository;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System.Net.Mime;



Log.Logger = new LoggerConfiguration()
.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
.Enrich.FromLogContext()
.WriteTo.Console()
.CreateLogger();

try
{
    Log.Information("starting the web host");

    var builder = WebApplication.CreateBuilder(args);
    builder.ConfigureSerilog();
    // Add services to the container.
    builder.Services.AddDbContext<EcommerceContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DockerConnectionString"]));
    builder.Services.AddAutoMapper(typeof(Program));
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));
    builder.ConfigureAuthentication();
    builder.Services.AddHealthChecks();

    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<IJwtUtilities, JwtUtilities>();
    builder.Services.AddScoped<IOrderService, OrderService>();
    builder.Services.AddScoped<UserService>();
    builder.Services.AddScoped<IUserService, CachedUserService>();
    builder.Services.AddScoped<ValidateCategoryExists>();
    builder.Services.AddScoped<ValidateProductExists>();

    builder.Services.AddMemoryCache();
    builder.Services.AddControllers();
    builder.Services.ConfigureApiVersioning();
    builder.Services.ConfigureVersionedApiExplorer();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.ConfigureSwaggerUIForAuthorizationWithToken();

    var app = builder.Build();

    await app.MigrateDatabaseIfDoesntExist();

    app.UseSerilogRequestLogging(configure =>
    {
        configure.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.000}ms";
    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
    }
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHealthChecks("/healthcheck", new HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            var result = JsonConvert.SerializeObject(new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(c => new
                {
                    check = c.Key,
                    result = c.Value.Status.ToString()
                }),
            });

            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(result);
        }
    });

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseMiddleware<GlobalErrorHandlingMiddleware>();

    app.MapControllers();
    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
return 0;

public partial class Program { }
