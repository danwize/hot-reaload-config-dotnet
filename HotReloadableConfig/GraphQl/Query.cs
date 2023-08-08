using HotChocolate.Data;
using HotReloadableConfig.Books;
using HotReloadableConfig.Books.Mongo;
using HotReloadableConfig.Books.Pocos;

namespace HotReloadableConfig.GraphQl;

public class Query
{
    public Task<Book?> GetBookById([Service] IBooksRepository booksBooksRepository, Guid id) => booksBooksRepository.GetByIdAsync(id);
    [UsePaging(MaxPageSize = 1)]
    // [UseProjection]
    [UseSorting]
    [UseFiltering]
    public IExecutable<Book> GetBooks([Service] IMongoBooksClient booksClient) => booksClient.Books.AsExecutable();
}