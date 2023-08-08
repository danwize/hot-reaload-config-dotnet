namespace HotReloadableConfig.Books.ConfigData;

public class MongoBooksConfigSource : IConfigurationSource
{
    private readonly WebApplicationBuilder _webAppBuilder;
    public MongoBooksConfigSource(WebApplicationBuilder appBuilder)
    {
        _webAppBuilder = appBuilder;
        ConfigureServices();
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {

        return _webAppBuilder.Services.BuildServiceProvider().GetRequiredService<MongoBooksConfigProvider>();
    }

    private void ConfigureServices()
    {
        _webAppBuilder.Services.Configure<BookMetadataConfigMongoDbConfig>(_webAppBuilder.Configuration.GetSection(nameof(BookMetadataConfigMongoDbConfig)));
        _webAppBuilder.Services.AddSingleton<IMongoBooksMetadataConfigClient, MongoBooksMetadataConfigClient>();
        _webAppBuilder.Services.AddSingleton<IBookMetadataConfigRepository, BookMetadataConfigRepository>();
        _webAppBuilder.Services.AddSingleton<MongoBooksConfigProvider>();
    }
}