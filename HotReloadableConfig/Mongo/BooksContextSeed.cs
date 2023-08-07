using HotReloadableConfig.Pocos;
using MongoDB.Driver;

namespace HotReloadableConfig.Mongo;

public class BooksContextSeed
{
    public static void SeedData(
        IMongoCollection<Book> booksCollection)
    {
        InsertRecords(booksCollection);
    }

    private static void InsertRecords(IMongoCollection<Book> booksCollection)
    {
        booksCollection.DeleteMany(_ => true);
        var dan = new Author(Name: "Dan Davis");
        booksCollection.InsertMany(
            new List<Book>
            {
                new (Id: Guid.Parse("4e27009c-7df5-4fbf-b6cc-81924f35af46"), Title: "Configs in DotNet", Author: dan),
                new (Id: Guid.Parse("5e27009c-7df5-4fbf-b6cc-81924f35af46"), Title: "DI in DotNet", Author: dan),
                new (Id: Guid.Parse("5e27009c-7df5-4fbf-b6cc-81924f35af47"), Title: "Another Book?", Author: dan)
            });
    }
}