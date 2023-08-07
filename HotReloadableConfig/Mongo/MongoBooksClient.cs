using HotReloadableConfig.Pocos;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace HotReloadableConfig.Mongo;

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