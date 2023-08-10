namespace HotReloadableConfig.Books.ConfigData;

//https://andrewlock.net/reloading-strongly-typed-options-in-asp-net-core-1-1-0/#if-you-don-t-like-to-use-ioptions
public class MongoBooksConfigProvider : ConfigurationProvider
{
    private readonly IBookMetadataConfigRepository _configRepository;

    public MongoBooksConfigProvider(IBookMetadataConfigRepository configRepository)
    {
        _configRepository = configRepository;
        RunScheduleJob(new CancellationToken());
    }

    public override void Load()
    {
        var data = _configRepository.GetConfig()
            .AsDictionary(keyPrefix:$"{nameof(BookMetadataConfig)}:");
        Data = data;
    }

    private async Task RunScheduleJob(CancellationToken token)
    {
        while(!token.IsCancellationRequested)
        {
            Load();
            OnReload();
            await Task.Delay(TimeSpan.FromSeconds(1), token);
        }
    }

}