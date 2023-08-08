using HotReloadableConfig.Books.Pocos;

namespace HotReloadableConfig.Mongo;

public class MongoDbConfiguration {
    public string ConnectionString {get;set;} = "";
    public string Database {get;set;} = "";
    public string CollectionName { get; set; } = "";
}