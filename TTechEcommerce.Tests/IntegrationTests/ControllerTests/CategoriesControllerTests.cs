﻿using Moq;
using System.Net;
using TTechEcommerceApi.Interface;
using TTechEcommerceApi.Model;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using EcommerceApi.Entities;
using TTechEcommerceApi.Controllers;
using Azure;
using Microsoft.AspNetCore.Mvc;

namespace TTechEcommerce.Tests.IntegrationTests.ControllerTests
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

        [Fact]
        public async Task GetById_IfFound_ReturnsObject()
        {
            var searchCategory = new Category() { Id = 2, Name = "A", Description = "A" };
            var mockCategories = new Category[]
            {
                searchCategory,
                new() { Id = 3 , Name = "B" , Description = "B"},
            };

            _factory.MockCategoryService.Setup(c => c.GetCategoryById(2)).Returns(Task.FromResult(searchCategory)!);

            var controller = new CategoriesController(_factory.MockCategoryService.Object);

            var result = await controller.GetById(2);
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(searchCategory, okObjectResult.Value);
        }

        [Fact]
        public async Task AddCategory_WithValidData_SavesCategory()
        {
            var newCategory = new Category() { Id = 1, Name = "A", Description = "A" };
            _factory.MockCategoryService
                .Setup(c => c.AddCategory(It.Is<Category>(c => c.Name == "A" && c.Id == 1 && c.Description == "A")))
                .Returns(Task.FromResult(newCategory))
                .Verifiable();


            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await _client.PostAsJsonAsync("/api/Categories", newCategory);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            _factory.MockCategoryService.VerifyAll();
        }

        [Fact]
        public async Task UpdateCategory_WithValidData_Return200StatusCode()
        {
            var updateCategory = new Category() { Id = 1, Name = "A", Description = "B" };

            _factory.MockCategoryService
                .Setup(c => c.UpdateCategory(It.Is<int>(i => i == 1), It.Is<Category>(c => c.Description == "B"))).Verifiable();

            //we use controller here because it has a custom action filter
            //and according to microsoft docs we have to test the custom filter
            //seperate from the controller so we don't have a choice but to use the 
            //controller action in isolation
            var controller = new CategoriesController(_factory.MockCategoryService.Object);

            var result = await controller.Update(1, updateCategory);
            Assert.IsType<OkResult>(result);

            _factory.MockCategoryService.VerifyAll();
        }

        [Fact]
        public async Task DeleteCategory_WithValidData_ReturnsNoContentStatus()
        {
            var updateCategory = new Category() { Id = 1, Name = "A", Description = "B" };

            _factory.MockCategoryService
                .Setup(c => c.DeleteCategory(It.Is<int>(i => i == 1))).Verifiable();

            var controller = new CategoriesController(_factory.MockCategoryService.Object);

            var result = await controller.Delete(1);
            Assert.IsType<NoContentResult>(result);

            _factory.MockCategoryService.VerifyAll();
        }

        public void Dispose()
        {
            _factory.Dispose();
            _client.Dispose();
        }
    }
}