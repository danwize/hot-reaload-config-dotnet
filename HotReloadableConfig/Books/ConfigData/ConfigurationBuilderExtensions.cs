namespace HotReloadableConfig.Books.ConfigData;

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