using HotReloadableConfig.Pocos;

namespace HotReloadableConfig.Repositories;

public interface IBooksRepository
{
    Task<Book?> GetByIdAsync(Guid id);

    Task<Book> UpdateAsync(Book book);
    Task<IEnumerable<Book>> GetAllAsync();

}