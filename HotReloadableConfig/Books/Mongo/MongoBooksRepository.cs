using HotReloadableConfig.Books.ConfigData;
using HotReloadableConfig.Books.Pocos;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HotReloadableConfig.Books.Mongo;

public class MongoBooksRepository : IBooksRepository
{
    private readonly IMongoBooksClient _mongoBooksClient;
    private readonly IOptions<BookMetadataConfig> _bookOptions;
    private BookMetadataConfig BookConfig => _bookOptions.Value;


    private FilterDefinition<Book> GetByIdFilter(Guid id)
    {
        return Builders<Book>.Filter.Eq(_ => _.Id, id);
    }

    public MongoBooksRepository(IMongoBooksClient mongoBooksClient, IOptions<BookMetadataConfig> bookOptions)
    {
        _mongoBooksClient = mongoBooksClient;
        _bookOptions = bookOptions;
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        var result = await _mongoBooksClient.Books.Find(_ => _.Id == id).FirstOrDefaultAsync();

        if (BookConfig.ShouldAddLastUpdate)
        {
            return result with {LastModifiedDate = DateTime.UtcNow};
        }

        return result;
    }

    public async Task<Book> UpdateAsync(Book book)
    {
        var updateDefinition = Builders<Book>.Update.Set(_ => _.Title, book.Title);
        return await _mongoBooksClient.Books.FindOneAndUpdateAsync(GetByIdFilter(book.Id), updateDefinition, new FindOneAndUpdateOptions<Book, Book> { ReturnDocument = ReturnDocument.After });
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    { 
        var result = await (await _mongoBooksClient.Books.FindAsync(_ => true)).ToListAsync();

        return BookConfig.ShouldAddLastUpdate ? result.Select(x => x with {LastModifiedDate = DateTime.UtcNow}) : result;
    }
}