using EcommerceApi.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog;
using TTechEcommerceApi.Extensions;
using TTechEcommerceApi.Filters.ActionFilters;
using TTechEcommerceApi.Helper;
using TTechEcommerceApi.Interface;
using TTechEcommerceApi.Middlewares;
using TTechEcommerceApi.Repository;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("starting the web host");

    var builder = WebApplication.CreateBuilder(args);

    builder.ConfigureSerilog();

    // Add services to the container.
    builder.Services.AddDbContext<EcommerceContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));
    builder.Services.AddAutoMapper(typeof(Program));
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));
    builder.ConfigureAuthentication();

    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<IJwtUtilities, JwtUtilities>();
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

    app.UseSerilogRequestLogging(configure =>
    {
        configure.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.000}ms";
    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

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

