using HotReloadableConfig.Books;
using HotReloadableConfig.Books.ConfigData;
using HotReloadableConfig.Books.Mongo;
using HotReloadableConfig.GraphQl;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IMongoBooksClient, MongoBooksClient>();
builder.Services.AddSingleton<IBooksRepository, MongoBooksRepository>();

//set up config
builder.Configuration.AddBookConfiguration(builder);
builder.Services.Configure<BooksDbConfiguration>(builder.Configuration.GetSection(nameof(BooksDbConfiguration)));
// builder.Services.Configure<BookMetadataConfig>()
builder.Services.Configure<BookMetadataConfig>(builder.Configuration.GetSection(nameof(BookMetadataConfig)));

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutate>()
    .AddMongoDbFiltering()
    .AddMongoDbSorting()
    .AddMongoDbPagingProviders()
    .AddMongoDbProjections();


var app = builder.Build();

app.MapGraphQL();

app.Run();




