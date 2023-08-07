using HotReloadableConfig.GraphQl;
using HotReloadableConfig.Mongo;
using HotReloadableConfig.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IMongoBooksClient, MongoBooksClient>();
builder.Services.AddSingleton<IBooksRepository, MongoBooksRepository>();

//set up config
builder.Services.Configure<BooksDbConfiguration>(builder.Configuration.GetSection(nameof(BooksDbConfiguration)));

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();


var app = builder.Build();

app.MapGraphQL();

app.Run();




