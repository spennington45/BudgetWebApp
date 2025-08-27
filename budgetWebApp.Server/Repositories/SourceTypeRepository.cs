using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace budgetWebApp.Server.Repositories
{
    public class SourceTypeRepository : ISourceTypeRepository
    {
        private readonly BudgetContext _context;

        public SourceTypeRepository(BudgetContext context)
        {
            _context = context;
        }

        public async Task<SourceType> AddSourceTypeAsync(SourceType sourceType)
        {
            var newSourceType = await _context.SourceTypes.AddAsync(sourceType);
            await _context.SaveChangesAsync();
            return newSourceType.Entity;
        }

        public async Task DeleteSourceTypeAsync(long id)
        {
            var sourceType = await GetSourceTypeBySourceTypeIdAsync(id);
            if (sourceType != null) {
                _context.SourceTypes.Remove(sourceType);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<SourceType> GetSourceTypeBySourceTypeIdAsync(long id)
        {
            return await _context.SourceTypes.FirstOrDefaultAsync(st => st.SourceTypeId == id);
        }

        public async Task<IEnumerable<SourceType>> GetSourceTypesAsync()
        {
            return await _context.SourceTypes.ToListAsync();
        }

        public async Task<SourceType> UpdateSourceType(SourceType sourceType)
        {
            var updatedSourceType = _context.SourceTypes.Update(sourceType);
            await _context.SaveChangesAsync();
            return updatedSourceType.Entity;
        }
    }
}
