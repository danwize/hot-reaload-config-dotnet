using HotReloadableConfig.Books.Pocos;
using HotReloadableConfig.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace HotReloadableConfig.Books.Mongo;

public class MongoBooksClient : MongoCollectionClient, IMongoBooksClient
{
    protected sealed override string CollectionName => $"{nameof(Book)}s";

    public MongoBooksClient(IOptions<BooksDbConfiguration> mongoDbConfiguration)
    {
        var pack = new ConventionPack();
        pack.AddMemberMapConvention("nullShouldBeNotSet", m =>
        {
            m.SetIgnoreIfNull(true); //don't save null 
        });

        // pack.Add(new convention);

        


        ConventionRegistry.Register("Books Convention", pack, t => t.FullName.StartsWith(typeof(Book).Namespace));
        BsonClassMap.RegisterClassMap<Book>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
            //serialize the guid id as a string.  If you don't do this, the GUIDs will get serialized as a binary and are very hard to read in the mongo collection.
            cm.MapProperty(x => x.Id).SetSerializer(new GuidSerializer(BsonType.String));
        });




        var client = new MongoClient(mongoDbConfiguration.Value.ConnectionString);
        var database = client.GetDatabase(mongoDbConfiguration.Value.Database);

        Books = database.GetCollection<Book>(CollectionName);

        BooksContextSeed.SeedData(Books);
    }

    public IMongoCollection<Book> Books { get; }
}