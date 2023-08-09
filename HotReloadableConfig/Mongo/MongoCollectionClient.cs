using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HotReloadableConfig.Mongo;

public abstract class MongoCollectionClient<T>
{
    protected string CollectionName { get; }
    protected MongoDbConfiguration DbConfig => _dbConfig.Value;
    private bool _isInitialized = false;

    private readonly IOptions<MongoDbConfiguration> _dbConfig;
    protected abstract List<T> SeedRecords { get; }

    private readonly IMongoCollection<T> _collection;


    protected MongoClient Client { get; }
    protected IMongoDatabase Database { get; }


    public IMongoCollection<T> Collection
    {
        get
        {
            if (_isInitialized) return _collection;
            SeedData();
            _isInitialized = true;
            return _collection;
        }
    }

    protected MongoCollectionClient(IOptions<MongoDbConfiguration> config)
    {
        //do the object init stuff before connecting to the database
        // mongoConventionRegistration();
        _dbConfig = config;
        Client = new MongoClient(DbConfig.ConnectionString);
        CollectionName = string.IsNullOrWhiteSpace(config.Value.CollectionName) ? 
            $"{typeof(T).Name}s" : 
            config.Value.CollectionName;
        Database = Client.GetDatabase(DbConfig.Database);
        _collection = Database.GetCollection<T>(CollectionName);


    }

    private 

    protected virtual void SeedData()
    {
        if(_collection.CountDocuments(FilterDefinition<T>.Empty) > 0)
        {
            //don't seed that data if there is already some data in there.
            return;
        }

        // _collection.DeleteMany(_ => true);
        _collection.InsertMany(SeedRecords);
    }

}