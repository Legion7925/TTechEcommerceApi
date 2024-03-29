﻿using EcommerceApi.Entities;
using Microsoft.EntityFrameworkCore;
using TTechEcommerceApi.Helper;
using TTechEcommerceApi.Interface;
using TTechEcommerceApi.Model;

namespace TTechEcommerceApi.Repository
{
    public class ProductService : IProductService
    {
        private readonly EcommerceContext context;

        public ProductService(EcommerceContext context)
        {
            this.context = context;
        }

        public IEnumerable<Product> GetAll(ProdudctQueryParametersModel queryParameters)
        {
            var products = context.Products.AsNoTracking();
            if (queryParameters.MinPrice is not null) products = products.Where(p => p.Price >= queryParameters.MinPrice);
            if (queryParameters.MaxPrice is not null) products = products.Where(p => p.Price <= queryParameters.MaxPrice);
            return products.Skip((queryParameters.Page - 1) * queryParameters.Size).Take(queryParameters.Size).ToArray();
        }

        public async Task<Product> AddProduct(Product product)
        {
            var categoryExists = context.Categories.Any(i => i.Id == product.CategoryId);
            if (!categoryExists)
                throw new TTechException("Category with the specified category Id not found !");

            context.Products.Add(product);
            await context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> GetProductById(int productId)
        {
            return await context.Products.FirstOrDefaultAsync(i => i.Id == productId);
        }

        public async Task UpdateProduct(int productId, Product product)
        {
            var findProduct = await GetProductById(productId);

            findProduct!.Color = product.Color;
            findProduct.Price = product.Price;
            findProduct.ImagePath = product.ImagePath;
            findProduct.Name = product.Name;
            findProduct.IsAvailable = product.IsAvailable;
            findProduct.CategoryId = product.CategoryId;

            await context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int productId)
        {
            var findProduct = await GetProductById(productId);
            context.Products.Remove(findProduct!);
            await context.SaveChangesAsync();
        }
    }
}
