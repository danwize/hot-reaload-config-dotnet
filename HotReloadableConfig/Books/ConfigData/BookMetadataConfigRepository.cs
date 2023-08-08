using MongoDB.Driver;

namespace HotReloadableConfig.Books.ConfigData;

public class BookMetadataConfigRepository : IBookMetadataConfigRepository
{
    private readonly IMongoBooksMetadataConfigClient _mongoConfigClient;

    public BookMetadataConfigRepository(IMongoBooksMetadataConfigClient mongoConfigClient)
    {
        _mongoConfigClient = mongoConfigClient;
    }


    public BookMetadataConfig GetConfig()
    {
        return _mongoConfigClient.Configs.Find(_=>true).Limit(1).SingleOrDefault();
    }
}