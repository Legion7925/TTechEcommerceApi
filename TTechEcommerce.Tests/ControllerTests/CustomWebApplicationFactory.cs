using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTechEcommerceApi.Interface;

namespace TTechEcommerce.Tests.ControllerTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<ICategoryService> MockCategoryService { get; }

        public CustomWebApplicationFactory()
        {
            MockCategoryService = new Mock<ICategoryService>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services =>
            {
                //doesn't matter what we determine for the life time of the object since the
                //_factor object gets disposed after each test call
                services.AddSingleton(MockCategoryService.Object);
            });
        }
    }
}
