using HotReloadableConfig.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace HotReloadableConfig.Books.ConfigData;

public class MongoBooksMetadataConfigClient : MongoCollectionClient<BookMetadataConfig>, IMongoBooksMetadataConfigClient
{
    static MongoBooksMetadataConfigClient()
    {
        var pack = new ConventionPack();
        pack.AddMemberMapConvention("nullShouldBeNotSet", m =>
        {
            m.SetIgnoreIfNull(true); //don't save null 
        });
        
        //More targeted way to add convention, but not have to specify every class and every property
        pack.Add(new GuidAsStringRepresentationConvention());
        pack.Add(new IgnoreExtraElementsConvention(true));
        
        
        ConventionRegistry.Register("Books Config Convention", pack, t => t.FullName.StartsWith(typeof(BookMetadataConfig).Namespace));
    }

    public MongoBooksMetadataConfigClient(IOptions<BookMetadataConfigMongoDbConfig> config) : base(config)
    {
        // var pack = new ConventionPack();
        // pack.AddMemberMapConvention("nullShouldBeNotSet", m =>
        // {
        //     m.SetIgnoreIfNull(true); //don't save null 
        // });
        //
        // //More targeted way to add convention, but not have to specify every class and every property
        // pack.Add(new GuidAsStringRepresentationConvention());
        // pack.Add(new IgnoreExtraElementsConvention(true));
        //
        //
        // ConventionRegistry.Register("Books Config Convention", pack, t => t.FullName.StartsWith(typeof(BookMetadataConfig).Namespace));
    }

    protected override List<BookMetadataConfig> SeedRecords => new()
    {
        new BookMetadataConfig(Guid.NewGuid(), true)
    };

    public IMongoCollection<BookMetadataConfig> Configs => Collection;
}