using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;

public interface IMongoBooksClient
{
    IMongoCollection<Book> Books{get;}
}

public class MongoDbConfiguration {
    public string ConnectionString {get;set;} = "";
    public string Database {get;set;} = "";
}

public class BooksDbConfiguration : MongoDbConfiguration
{

}

public class MongoBooksClient : IMongoBooksClient 
{
    private const string BooksCollectionName = $"{nameof(Book)}s";

    public MongoBooksClient(IOptions<BooksDbConfiguration> mongoDbConfiguration)
    {
        var pack = new ConventionPack();
        pack.AddMemberMapConvention("nullShouldBeNotSet", m =>
        {
            m.SetIgnoreIfNull(true); //don't save null values
        });


        ConventionRegistry.Register("Books Convention", pack, t=> t.FullName.StartsWith("HotReloadableConfig.Pocos"));
        BsonClassMap.RegisterClassMap<Book>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        });

        var client = new MongoClient(mongoDbConfiguration.Value.ConnectionString);
        var database = client.GetDatabase(mongoDbConfiguration.Value.Database);

        Books = database.GetCollection<Book>(BooksCollectionName);

        BooksContextSeed.SeedData(Books);
    }

    public IMongoCollection<Book> Books{get;}
}


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
        var dan = new Author() {
            Name = "Dan Davis",
        };
        booksCollection.InsertMany(
            new List<Book>
            {
                new ()
                {
                    Id = Guid.Parse("4e27009c-7df5-4fbf-b6cc-81924f35af46"),
                    Title = "Configs in DotNet",
                    Author = dan
                },
                new ()
                {
                    Id = Guid.Parse("5e27009c-7df5-4fbf-b6cc-81924f35af46"),
                    Title = "DI in DotNet",
                    Author = dan
                }
            });
    }
}