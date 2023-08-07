using HotReloadableConfig.Pocos;
using MongoDB.Driver;

namespace HotReloadableConfig.Mongo;

public interface IMongoBooksClient
{
    IMongoCollection<Book> Books{get;}
}

public class BooksDbConfiguration : MongoDbConfiguration
{

}