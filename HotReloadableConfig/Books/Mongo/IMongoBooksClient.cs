using HotReloadableConfig.Books.Pocos;
using MongoDB.Driver;

namespace HotReloadableConfig.Books.Mongo;

public interface IMongoBooksClient
{
    IMongoCollection<Book> Books { get; }
}