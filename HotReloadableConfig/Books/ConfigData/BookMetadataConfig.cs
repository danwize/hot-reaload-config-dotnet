namespace HotReloadableConfig.Books.ConfigData;

//https://learn.microsoft.com/en-us/dotnet/core/extensions/custom-configuration-provider

public record BookMetadataConfig
{
    public BookMetadataConfig(Guid id, bool shouldAddLastUpdate)
    {
        Id = id;
        ShouldAddLastUpdate = shouldAddLastUpdate;
    }

    public BookMetadataConfig()
    {
    
    }

    public Guid Id { get; init; }
    public bool ShouldAddLastUpdate { get; init; }
}