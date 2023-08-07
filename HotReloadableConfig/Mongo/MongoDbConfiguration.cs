using HotReloadableConfig.Books.Pocos;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HotReloadableConfig.Mongo;

public class MongoDbConfiguration {
    public string ConnectionString {get;set;} = "";
    public string Database {get;set;} = "";
    public string CollectionName { get; set; } = "";
}

public abstract class MongoCollectionClient<T>
{
    protected string CollectionName { get; }
    protected MongoDbConfiguration DbConfig => _dbConfig.Value;

    private readonly IOptions<MongoDbConfiguration> _dbConfig;
    protected abstract List<T> SeedRecords { get; }



    protected MongoClient Client { get; }
    protected IMongoDatabase Database { get; }

    protected IMongoCollection<T> Collection { get; }

    protected MongoCollectionClient(IOptions<MongoDbConfiguration> config)
    {
        _dbConfig = config;
        Client = new MongoClient(DbConfig.ConnectionString);
        CollectionName = string.IsNullOrWhiteSpace(config.Value.CollectionName) ? 
            $"{typeof(T).Name}s" : 
            config.Value.CollectionName;
        Database = Client.GetDatabase(DbConfig.Database);
        Collection = Database.GetCollection<T>(CollectionName);

        Init();

    }

    //moved 
    private void Init()
    {
        //todo: moved ths virtual cal to an init function because of a warning about virtual call in constructor.  I think that doesn't solve the problem???
        //initial testing shows it is ok.
        SeedData();
    }

    protected virtual void SeedData()
    {
        if(Collection.CountDocuments(FilterDefinition<T>.Empty) > 0)
        {
            //don't seed that data if there is already some data in there.
            return;
        }

        Collection.DeleteMany(_ => true);
        Collection.InsertMany(SeedRecords);


    }

}