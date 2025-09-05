using budgetWebApp.Server.Models;

namespace budgetWebApp.Server.Interfaces
{
    public interface ISourceTypeRepository
    {
        Task<IEnumerable<SourceType>> GetSourceTypesAsync();

        Task<SourceType> GetSourceTypeBySourceTypeIdAsync(long id);

        Task<SourceType> AddSourceTypeAsync(SourceType sourceType);

        Task<SourceType> UpdateSourceType(SourceType sourceType);

        Task<bool> DeleteSourceTypeAsync(long id);
    }
}
