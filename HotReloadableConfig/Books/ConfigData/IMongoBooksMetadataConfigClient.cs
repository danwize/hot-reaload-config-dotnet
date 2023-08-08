using MongoDB.Driver;

namespace HotReloadableConfig.Books.ConfigData;

public interface IMongoBooksMetadataConfigClient
{
    public IMongoCollection<BookMetadataConfig> Configs { get; }
}