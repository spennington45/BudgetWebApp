using budgetWebApp.Server.Models;

namespace budgetWebApp.Server.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategorysAsync();

        Task<Category> GetCategoryByCategoryIdAsync(long id);

        Task<Category> AddCategoryAsync(Category category);

        Task<Category> UpdateCategory(Category category);

        Task DeleteCategoryAsync(long id);
    }
}
