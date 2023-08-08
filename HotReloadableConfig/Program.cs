using HotReloadableConfig.Books;
using HotReloadableConfig.Books.ConfigData;
using HotReloadableConfig.Books.Mongo;
using HotReloadableConfig.GraphQl;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IMongoBooksClient, MongoBooksClient>();
builder.Services.AddSingleton<IBooksRepository, MongoBooksRepository>();

//set up config
builder.Services.Configure<BooksDbConfiguration>(builder.Configuration.GetSection(nameof(BooksDbConfiguration)));
// builder.Services.Configure<BookMetadataConfig>()
builder.Configuration.AddBookConfiguration(builder);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();


var app = builder.Build();

app.MapGraphQL();

app.Run();




