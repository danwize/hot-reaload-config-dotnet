using HotReloadableConfig.Books.Pocos;

namespace HotReloadableConfig.Books;

public interface IBooksRepository
{
    Task<Book?> GetByIdAsync(Guid id);

    Task<Book> UpdateAsync(Book book);
    Task<IEnumerable<Book>> GetAllAsync();

    Task<Book> AddAsync(Book book);

}