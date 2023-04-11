using EcommerceApi.Entities;
using Microsoft.EntityFrameworkCore;
using TTechEcommerceApi.Interface;
using TTechEcommerceApi.Model;

namespace TTechEcommerceApi.Repository
{
    public class CategoryService : ICategoryService
    {
        private readonly EcommerceContext context;

        public CategoryService(EcommerceContext context)
        {
            this.context = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return context.Categories.AsNoTracking();
        }

        public async Task<Category?> GetCategoryById(int categoryId)
        {
            return await context.Categories.FirstOrDefaultAsync(i => i.Id == categoryId);
        }

        public async Task<Category> AddCategory(Category category)
        {
            context.Categories.Add(category);
            await context.SaveChangesAsync();
            return category;
        }

        public async Task UpdateCategory(int categoryId, Category category)
        {
            var findCategory = await GetCategoryById(categoryId);
            findCategory!.Name = category.Name;
            findCategory.Description = category.Description;
            await context.SaveChangesAsync();
        }

        public async Task DeleteCategory(int categoryId)
        {
            var category = await GetCategoryById(categoryId);
            context.Categories.Remove(category!);
            await context.SaveChangesAsync();
        }
    }
}
