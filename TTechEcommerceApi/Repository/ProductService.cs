using EcommerceApi.Entities;
using Microsoft.EntityFrameworkCore;
using TTechEcommerceApi.Helper;
using TTechEcommerceApi.Interface;

namespace TTechEcommerceApi.Repository
{
    public class ProductService : IProductService
    {
        private readonly EcommerceContext context;
        public ProductService(EcommerceContext context)
        {
            this.context = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return context.Products.AsNoTracking();
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

        public async Task<Product?> UpdateProduct(int productId, Product product)
        {
            var findProduct = await GetProductById(productId);

            if (findProduct == null)
                return null;

            findProduct.Color = product.Color;
            findProduct.Price = product.Price;
            findProduct.ImagePath = product.ImagePath;
            findProduct.Name = product.Name;
            findProduct.IsAvailable = product.IsAvailable;
            findProduct.CategoryId = product.CategoryId;

            await context.SaveChangesAsync();
            return findProduct;
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            var findProduct = await GetProductById(productId);
            if (findProduct == null)
                return false;

            context.Products.Remove(findProduct);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
