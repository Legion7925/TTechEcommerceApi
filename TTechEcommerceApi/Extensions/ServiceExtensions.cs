using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using TTechEcommerceApi.Authentication;
using TTechEcommerceApi.Helper;

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

    }
}
