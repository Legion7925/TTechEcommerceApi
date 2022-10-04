using EcommerceApi.Entities;

namespace TTechEcommerceApi.Interface
{
    public interface ICategoryService
    {
        Task<Category> AddCategory(Category category);
        IEnumerable<Category> GetAll();
        Task<Category?> GetCategoryById(int categoryId);
        Task<Category?> UpdateCategory(int categoryId, Category category);
        public Task<bool> DeleteCategory(int categoryId);
    }
}