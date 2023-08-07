namespace HotReloadableConfig.Mongo;

public class MongoDbConfiguration {
    public string ConnectionString {get;set;} = "";
    public string Database {get;set;} = "";
}

public abstract class MongoCollectionClient
{
    protected abstract string CollectionName { get; }
}