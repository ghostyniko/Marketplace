using Marketplace;
using Marketplace.ClassifiedAd;
using Marketplace.Domain.ClassifiedAd;
using Marketplace.Domain.Shared;
using Marketplace.Framework;
using Marketplace.Infrastructure;
using Raven.Client.Documents;
using Serilog;
using Marketplace.UserProfile;
using EventStore.ClientAPI;
using static Marketplace.ClassifiedAd.ReadModels;
using static Marketplace.UserProfile.ReadModels;
using Marketplace.Projections;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide;
using Raven.Client.Documents.Session;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var store = new DocumentStore
//{
//    Urls = new[] { "http://localhost:8080" },
//    Database = "Marketplace_Chapter8",
//    Conventions =
//    {
//        FindIdentityProperty = x => x.Name == "DbId"
//    }

//};
//store.Initialize();

//const string connectionString = "Host=localhost;Database=Marketplace_Chapter8;Username=postgres;Password = kokikoki";
//builder.Services
//    .AddDbContext<ClassifiedAdDbContext>(
//    options => options.UseNpgsql(connectionString));

var configuration = builder.Configuration;
var environment = builder.Environment;

var profanityClient = new ProfanityClient();

Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger();

var esConnection = EventStoreConnection.Create(
    configuration["eventStore:connectionString"],
 ConnectionSettings.Create().KeepReconnecting(),
 environment.ApplicationName);

esConnection.Connected += (@event, _args) =>
{
    Log.Information("Successfull connection");
};
esConnection.ErrorOccurred += (@event, _args) =>
{
    Log.Information("Error occured");
};
esConnection.Reconnecting += (@event, _args) =>
{
    Log.Information("Reconnection occured");
};
esConnection.AuthenticationFailed += (@event, _args) =>
{
    Log.Information("Auth failed");
};

var documentStore = ConfigureRavenDb(configuration);
Func<IAsyncDocumentSession> getSession =
 () => documentStore.OpenAsyncSession();

builder.Services.AddTransient(c => getSession());

var classifiedAdDetails = new List<ClassifiedAdDetails>();
builder.Services.AddSingleton<IEnumerable<ClassifiedAdDetails>>(classifiedAdDetails);
var userDetails = new List<UserDetails>();
builder.Services.AddSingleton<IEnumerable<UserDetails>>(userDetails);

var ravenDbCheckpointStore = new RavenDbCheckpointStore(getSession, "readmodel");

var projections = new IProjection[]
{
    new UserDetailsProjection(getSession),
    new ClassifiedAdDetailsProjection(getSession,async userId => (await getSession.GetUserDetails(userId))?.DisplayName),
    new ClassifiedAdUpcasters(esConnection,id=>"bla")
};
var subscription = new ProjectionsManager(esConnection,ravenDbCheckpointStore, projections);

var store = new EsAggregateStore(esConnection);
builder.Services.AddSingleton(esConnection);
builder.Services.AddSingleton<IAggregateStore>(store);

builder.Services.AddSingleton(new ClassifiedAdApplicationService(
store, new FixedCurrencyLookup()));

builder.Services.AddSingleton(new UserProfileApplicationService(
store, _text => profanityClient.CheckForProfacity(_text)));

builder.Services.AddSingleton<IHostedService>(
 new EventStoreService(esConnection, subscription));



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//app.EnsureDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "ClassifiedAds v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

 static IDocumentStore ConfigureRavenDb(IConfiguration configuration)
{
    var store = new DocumentStore
    {
        Urls = new[] { configuration["ravenDb:server"] },
        Database = configuration["ravenDb:database"]
    };
    store.Initialize();
    var record = store.Maintenance.Server.Send(
        new GetDatabaseRecordOperation(store.Database));
    if (record == null)
    {
        store.Maintenance.Server.Send(
            new CreateDatabaseOperation(new DatabaseRecord(store.Database)));
    }

    return store;
}