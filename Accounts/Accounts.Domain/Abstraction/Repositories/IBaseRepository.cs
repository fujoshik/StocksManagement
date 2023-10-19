namespace Accounts.Domain.Abstraction.Repositories
{
    public interface IBaseRepository
    {
        Task<TOutput> InsertAsync<TInput, TOutput>(TInput dto)
            where TOutput : new();
        Task<TOutput> GetByIdAsync<TOutput>(Guid id)
            where TOutput : new();
        Task<List<TOutput>> GetAllAsync<TOutput>()
            where TOutput : new();
        Task UpdateAsync<TInput>(Guid id, TInput dto);
        Task DeleteAsync(Guid id);
    }
}
