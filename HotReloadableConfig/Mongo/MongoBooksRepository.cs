using HotReloadableConfig.Pocos;
using HotReloadableConfig.Repositories;
using MongoDB.Driver;

namespace HotReloadableConfig.Mongo;

public class MongoBooksRepository : IBooksRepository
{
    private readonly IMongoBooksClient _mongoBooksClient;


    private FilterDefinition<Book> GetByIdFilter(Guid id)
    {
        return Builders<Book>.Filter.Eq(_ => _.Id, id);
    }

    public MongoBooksRepository(IMongoBooksClient mongoBooksClient)
    {
        _mongoBooksClient = mongoBooksClient;
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        // return await _recordContext.Records.Find(GetByIdFilter(deviceId)).FirstOrDefaultAsync();
        return await _mongoBooksClient.Books.Find(_ => _.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Book> UpdateAsync(Book book)
    {
        var updateDefinition = Builders<Book>.Update.Set(_ => _.Title, book.Title);
        return await _mongoBooksClient.Books.FindOneAndUpdateAsync(GetByIdFilter(book.Id), updateDefinition, new FindOneAndUpdateOptions<Book, Book> { ReturnDocument = ReturnDocument.After });
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await (await _mongoBooksClient.Books.FindAsync(_ => true)).ToListAsync();
    }
}