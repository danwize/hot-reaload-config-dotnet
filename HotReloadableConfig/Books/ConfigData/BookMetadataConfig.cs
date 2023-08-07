using HotReloadableConfig.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HotReloadableConfig.Books.ConfigData;

public record BookMetadataConfig(bool ShouldAddLastUpdate);

public class BookMetadataConfigMongoDbConfig : MongoDbConfiguration
{
}


public interface IBookMetadataConfigRepository
{
    Task<BookMetadataConfig> GetConfig();
}

public class BookMetadataConfigRepository : IBookMetadataConfigRepository
{
    private readonly IOptions<BookMetadataConfigMongoDbConfig> _config;

    public BookMetadataConfigRepository(IOptions<BookMetadataConfigMongoDbConfig> config, IMongoBooksMetadataConfigClient mongoConfigClient)
    {
        _config = config;
    }


    public Task<BookMetadataConfig> GetConfig()
    {
        throw new NotImplementedException();
    }
}

public interface IMongoBooksMetadataConfigClient
{
    public IMongoCollection<BookMetadataConfig> Config { get; }
}

public class MongoBooksMetadataConfigClient
{
    public MongoBooksMetadataConfigClient()
    {
        
    }
}