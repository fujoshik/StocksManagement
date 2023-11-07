using Accounts.Domain.Pagination;

namespace Accounts.Domain.Abstraction.Repositories
{
    public interface IBaseRepository
    {
        Task<TOutput> InsertAsync<TInput, TOutput>(TInput dto)
            where TOutput : new();
        Task<TOutput> GetByIdAsync<TOutput>(Guid id)
            where TOutput : new();
        Task<PaginatedResult<TOutput>> GetPageAsync<TOutput>(int pageNumber, int pageSize)
            where TOutput : new();
        Task<TOutput> UpdateAsync<TInput, TOutput>(Guid id, TInput dto)
            where TOutput : new();
        Task DeleteAsync(Guid id);
    }
}