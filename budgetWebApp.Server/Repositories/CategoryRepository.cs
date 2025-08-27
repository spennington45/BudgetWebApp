using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace budgetWebApp.Server.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BudgetContext _context;

        public CategoryRepository(BudgetContext context)
        {
            _context = context;
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            var newCategory = await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return newCategory.Entity;
        }

        public async Task DeleteCategoryAsync(long id)
        {
            var category = await GetCategoryByCategoryIdAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Category> GetCategoryByCategoryIdAsync(long id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<IEnumerable<Category>> GetCategorysAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            var updatedCategory = _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return updatedCategory.Entity;
        }
    }
}
