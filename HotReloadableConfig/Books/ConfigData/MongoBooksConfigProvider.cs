namespace HotReloadableConfig.Books.ConfigData;

public class MongoBooksConfigProvider : ConfigurationProvider
{
    private readonly IBookMetadataConfigRepository _configRepository;

    public MongoBooksConfigProvider(IBookMetadataConfigRepository configRepository)
    {
        _configRepository = configRepository;
    }

    public override void Load()
    {
        var data = _configRepository.GetConfig().AsDictionary(keyPrefix:$"{nameof(BookMetadataConfig)}:");
        Data = data;
    }
}