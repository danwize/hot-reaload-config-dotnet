using HotReloadableConfig.Books.Pocos;
using HotReloadableConfig.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace HotReloadableConfig.Books.Mongo;

public class MongoBooksClient : MongoCollectionClient<Book>, IMongoBooksClient
{
    static MongoBooksClient()
    {
        try
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


            ConventionRegistry.Register(typeof(MongoBooksClient).FullName, pack, t => t.Namespace == typeof(Book).Namespace);


            //This is a more targeted way to set class mapping.  This was replaced with the IgnoreExtraElementsConvention and applied to everything in the Book Namespace.
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
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    // private BooksDbConfiguration _configuration => _configurationOptions.Value;

    // private readonly IOptions<BooksDbConfiguration> _configurationOptions;

    public MongoBooksClient(IOptions<BooksDbConfiguration> mongoDbConfiguration) : base(mongoDbConfiguration)
    {

    }



    public IMongoCollection<Book> Books => Collection;

    protected override List<Book> SeedRecords
    {
        get
        {
            var dan = new Author(Name: "Dan Davis");

            return new List<Book>
            {
                new(Id: Guid.Parse("4e27009c-7df5-4fbf-b6cc-81924f35af46"), Title: "Configs in DotNet", Author: dan),
                new(Id: Guid.Parse("5e27009c-7df5-4fbf-b6cc-81924f35af46"), Title: "DI in DotNet", Author: dan),
                new(Id: Guid.Parse("5e27009c-7df5-4fbf-b6cc-81924f35af47"), Title: "Another Book?", Author: dan)
            };
        }
    }
}