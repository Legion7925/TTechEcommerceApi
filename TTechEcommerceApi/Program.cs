using EcommerceApi.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TTechEcommerceApi.Extensions;
using TTechEcommerceApi.Filters.ActionFilters;
using TTechEcommerceApi.Helper;
using TTechEcommerceApi.Interface;
using TTechEcommerceApi.MapperConfiguration;
using TTechEcommerceApi.Middlewares;
using TTechEcommerceApi.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EcommerceContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.ConfigureAuthentication();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IJwtUtilities, JwtUtilities>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUserService , CachedUserService>();
builder.Services.AddScoped<ValidateCategoryExists>();
builder.Services.AddScoped<ValidateProductExists>();

builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.ConfigureApiVersioning();
builder.Services.ConfigureVersionedApiExplorer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwaggerUIForAuthorizationWithToken();


var app = builder.Build();

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
