using Bazinga.AspNetCore.Authentication.Basic;
using EcommerceApi.Entities;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System;
using Testcontainers.MsSql;
using TTechEcommerce.Tests.Extensions;
using TTechEcommerceApi.Interface;

namespace TTechEcommerce.Tests.IntegrationTests.ControllerTests
{
    public class CustomWebApplicationFactory<TProgram, TDbContext> : WebApplicationFactory<TProgram>, IAsyncLifetime
        where TProgram : class where TDbContext : DbContext
    {
        private readonly MsSqlContainer _container;

        public Mock<ICategoryService> MockCategoryService { get; }

        public CustomWebApplicationFactory()
        {
            _container = new MsSqlBuilder()
                .WithPassword("123qwe!@#")
                .WithCleanUp(true)
                .Build();
            MockCategoryService = new Mock<ICategoryService>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var a = _container.GetConnectionString();
            builder.ConfigureTestServices(services =>
            {
                //doesn't matter what we determine for the life time of the object since the
                //_factor object gets disposed after each test call
                services.RemoveDbContext<TDbContext>();
                services.AddDbContext<TDbContext>(options => options.UseSqlServer(_container.GetConnectionString()));
                //services.EnsureDbCreated<TDbContext>();

                //mock authentication and authorization policy with bazinga basic authentication library
                services.AddAuthentication()
                 .AddBasicAuthentication(credentials =>Task.FromResult(credentials.username == "Test" && credentials.password == "test"));
                services.AddAuthorization(config =>
                {
                    config.DefaultPolicy = new AuthorizationPolicyBuilder(config.DefaultPolicy)
                                                .AddAuthenticationSchemes(BasicAuthenticationDefaults.AuthenticationScheme)
                                                .Build();
                });
                services.AddSingleton(MockCategoryService.Object);
            });
        }

        public async Task InitializeAsync()
        {
            await _container.StartAsync();
        }

        public new async Task DisposeAsync()
        {
            await _container.DisposeAsync();
        }
    }
}
