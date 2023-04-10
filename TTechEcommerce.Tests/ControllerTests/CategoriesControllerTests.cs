using Moq;
using System.Net;
using TTechEcommerceApi.Interface;
using TTechEcommerceApi.Model;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using EcommerceApi.Entities;

namespace TTechEcommerce.Tests.ControllerTests
{
    public class CategoriesControllerTests : IDisposable
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private string _token = string.Empty;

        public CategoriesControllerTests()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
            Authorize().Wait();
        }

        private async Task Authorize()
        {
            var response = await _client.PostAsJsonAsync("/api/Users/Login", new AuthenticateRequestModel { Username = "root", Password = "123" });
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var data = JsonConvert.DeserializeObject<AuthenticateResponseModel>(await response.Content.ReadAsStringAsync());
            _token = data!.JwtToken!;
        }

        [Fact]
        public async Task GetCategories_WhenCalled_ReturnsAllCategories()
        {
            var mockCategories = new Category[]
            {
                new() { Id = 1 , Name = "A" , Description = "A"},
                new() { Id = 2 , Name = "B" , Description = "B"},
            };

            _factory.MockCategoryService.Setup(c => c.GetAll()).Returns(mockCategories);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await _client.GetAsync("/api/Categories");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var data = JsonConvert.DeserializeObject<IEnumerable<Category>>(await response.Content.ReadAsStringAsync())!;

            Assert.Collection(data,
            c =>
            {
                Assert.Equal("A", c.Name);
                Assert.Equal("A", c.Description);
                Assert.Equal(1, c.Id);
            },
            c =>
            {
                Assert.Equal("B", c.Name);
                Assert.Equal("B", c.Description);
                Assert.Equal(2, c.Id);
            });
        }

        public void Dispose()
        {
            _factory.Dispose();
            _client.Dispose();
        }
    }
}
