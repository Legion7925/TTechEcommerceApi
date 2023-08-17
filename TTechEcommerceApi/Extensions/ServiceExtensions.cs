using EcommerceApi.Entities;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nest;
using Serilog;
using System.Text;
using TTechEcommerceApi.Authentication;

namespace TTechEcommerceApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(1.0),
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = builder.Configuration[$"{JwtSettings.SectionName}:Issuer"],
                    ValidAudience = builder.Configuration[$"{JwtSettings.SectionName}:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[$"{JwtSettings.SectionName}:Secret"]!))
                };
            });
        }

        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(builder =>
            {
                builder.ReportApiVersions = true;
                builder.DefaultApiVersion = new ApiVersion(1, 0);
                builder.AssumeDefaultVersionWhenUnspecified = true;
            });
        }

        public static void ConfigureVersionedApiExplorer(this IServiceCollection services)
        {
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }

        public static void ConfigureSwaggerUIForAuthorizationWithToken(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter the token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }

        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, services, configuration) => configuration
               .ReadFrom.Configuration(context.Configuration)
               .ReadFrom.Services(services)
               .Enrich.FromLogContext());
        }

        public static void ConfigureFluentValidation(this WebApplicationBuilder builder)
        {
            builder.Services.AddFluentValidationAutoValidation();
        }

        public static async Task MigrateDatabaseIfDoesntExist(this WebApplication builder)
        {
            using (var scope = builder.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<EcommerceContext>();
                await db.Database.MigrateAsync();
            }
        }

        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["ElasticSettings:BaseUrl"] ?? string.Empty;
            var defaultIndex = configuration["ElasticSettings:DefaultIndex"] ?? string.Empty;

            //this is a poor implementation only for test purpose
            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex)
                .CertificateFingerprint(configuration["ElasticSettings:Certificate"])
                .BasicAuthentication(configuration["ElasticSettings:Username"], configuration["ElasticSettings:Password"]);

            AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, defaultIndex);
        }

        private static void CreateIndex(ElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName,
                index => index.Map<Product>(x => x.AutoMap())
            );
        }

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings
                .DefaultMappingFor<Product>(m => m
                    .Ignore(p => p.Price)
                    .Ignore(p => p.ImagePath)
                );
        }
    }
}
