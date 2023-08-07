using HotReloadableConfig.Books.Pocos;
using HotReloadableConfig.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace HotReloadableConfig.Books.Mongo;

public class MongoBooksClient : MongoCollectionClient<Book>, IMongoBooksClient
{

    // private BooksDbConfiguration _configuration => _configurationOptions.Value;

    // private readonly IOptions<BooksDbConfiguration> _configurationOptions;

    public MongoBooksClient(IOptions<BooksDbConfiguration> mongoDbConfiguration) : base(mongoDbConfiguration)
    {


        //https://kevsoft.net/2020/06/25/storing-guids-as-strings-in-mongodb-with-csharp.html

        //globally use the string serializer for guids.
        //BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));


        var pack = new ConventionPack();
        pack.AddMemberMapConvention("nullShouldBeNotSet", m =>
        {
            m.SetIgnoreIfNull(true); //don't save null 
        });

        //More targeted way to add convention, but not have to specify every class and every property
        pack.Add(new GuidAsStringRepresentationConvention());
        pack.Add(new IgnoreExtraElementsConvention(true));


        ConventionRegistry.Register("Books Convention", pack, t => t.FullName.StartsWith(typeof(Book).Namespace));
        
        //This is a more targeted way to set class mapping.  This was replaced with the IgnoreExtraElementsConvention and applied to everything in the Book Nmespace.
        // BsonClassMap.RegisterClassMap<Book>(cm =>
        // {
        //     cm.AutoMap();
        //     cm.SetIgnoreExtraElements(true);
        //     //serialize the guid id as a string.  If you don't do this, the GUIDs will get serialized as a binary and are very hard to read in the mongo collection.
        //     //prefer specifying serializers and mongo conventions here rather than decorators on the POCOs
        //     // cm.MapProperty(x => x.Id).SetSerializer(new GuidSerializer(BsonType.String));
        //     
        //     //wrap in a nullable serializer to handle nullable guid.
        //     // cm.MapProperty(x => x.Id).SetSerializer(new NullableSerializer<Guid>(new GuidSerializer(BsonType.String)));
        // });

    }

    public IMongoCollection<Book> Books => Collection;
    protected override List<Book> SeedRecords => BooksContextSeed.Books;
}