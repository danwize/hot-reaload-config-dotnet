using System.Reflection;
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
    BookMetadataConfig GetConfig();
}

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

public interface IMongoBooksMetadataConfigClient
{
    public IMongoCollection<BookMetadataConfig> Configs { get; }
}

public class MongoBooksMetadataConfigClient : MongoCollectionClient<BookMetadataConfig>, IMongoBooksMetadataConfigClient
{
    public MongoBooksMetadataConfigClient(IOptions<BookMetadataConfigMongoDbConfig> config) : base(config)
    {
    }

    protected override List<BookMetadataConfig> SeedRecords => new()
    {
        new BookMetadataConfig(true)
    };

    public IMongoCollection<BookMetadataConfig> Configs => Collection;
}

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

public class MongoBooksConfigProvider : ConfigurationProvider
{
    private readonly IBookMetadataConfigRepository _configRepository;

    public MongoBooksConfigProvider(IBookMetadataConfigRepository configRepository)
    {
        _configRepository = configRepository;
    }

    public override void Load()
    {
        Data = _configRepository.GetConfig().AsDictionary();
    }
}

public static class ObjectExtensions
{
    public static T ToObject<T>(this IDictionary<string, object> source)
        where T : class, new()
    {
        var someObject = new T();
        var someObjectType = someObject.GetType();

        foreach(var item in source)
        {
            someObjectType
                .GetProperty(item.Key)
                .SetValue(someObject, item.Value, null);
        }

        return someObject;
    }

    public static IDictionary<string, string?> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
    {
        return source.GetType().GetProperties(bindingAttr).ToDictionary
        (
            propInfo => propInfo.Name,
            propInfo => propInfo.GetValue(source, null)?.ToString()
        );

    }
}

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddBookConfiguration(
        this IConfigurationBuilder builder, WebApplicationBuilder webAppBuilder)
    {
        // var tempConfig = builder.Build();
        // var connectionString =
        //     tempConfig.GetConnectionString("WidgetConnectionString");

        return builder.Add(new MongoBooksConfigSource(webAppBuilder));
    }
}