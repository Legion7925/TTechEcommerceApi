using EcommerceApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace TTechEcommerceApi.Repository
{
    public class CategoryService
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


        public async Task<Category?> UpdateCategory(int categoryId, Category category)
        {
            var findCategory = await GetCategoryById(categoryId);
            if (findCategory != null)
            {
                findCategory.Name = category.Name;
                findCategory.Description = category.Description;
                await context.SaveChangesAsync();

                return category;
            }
            else
            {
                return null;
            }
        }
    }
}
