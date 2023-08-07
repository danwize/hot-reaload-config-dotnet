using HotReloadableConfig.Pocos;
using HotReloadableConfig.Repositories;

namespace HotReloadableConfig.GraphQl;

public class Query
{
    public Task<Book?> GetBookById([Service] IBooksRepository booksBooksRepository, Guid id) => booksBooksRepository.GetByIdAsync(id);
}