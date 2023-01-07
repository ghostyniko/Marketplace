using Marketplace;

using Marketplace.Infrastructure;
using Raven.Client.Documents;
using Serilog;

using EventStore.ClientAPI;

using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide;
using Raven.Client.Documents.Session;
using Marketplace.Infrastructure.Profanity;
using Marketplace.Modules.Images;
using Marketplace.EventStore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Marketplace.WebApi;
using Marketplace.Ads;
using Marketplace.Infrastructure.Currency;
using Marketplace.Infrastructure.RavenDb;
using static Marketplace.Infrastructure.RavenDb.Configuration;
using Marketplace.Users;

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

var esConnection = EventStoreConnection.Create(
    configuration["eventStore:connectionString"],
 ConnectionSettings.Create().KeepReconnecting(),
 environment.ApplicationName);

var profanityClient = new ProfanityClient();

var documentStore = ConfigureRavenDb(
   configuration["ravenDb:server"]
);

builder.Services.AddSingleton(new ImageQueryService(ImageStorage.GetFile));
builder.Services.AddSingleton(esConnection);
builder.Services.AddSingleton(documentStore);
builder.Services.AddSingleton<IHostedService, EventStoreService>();

//builder.Services
//       .AddAuthentication(
//           CookieAuthenticationDefaults.AuthenticationScheme
//            )
//       .AddCookie();

builder.Services
    .AddMvcCore(
        options => options.Conventions.Add(new CommandConvention())
    )
    .AddApplicationPart(assembly: builder.GetType().Assembly)
    .AddAdsModule(
        "ClassifiedAds",
        new FixedCurrencyLookup(),
        ImageStorage.UploadFile
    )
    .AddUsersModule(
        "Users",
         profanityClient.CheckForProfanity
     );

//Log.Logger = new LoggerConfiguration()
//        .WriteTo.Console()
//        .CreateLogger();


//esConnection.Connected += (@event, _args) =>
//{
//    Log.Information("Successfull connection");
//};
//esConnection.ErrorOccurred += (@event, _args) =>
//{
//    Log.Information("Error occured");
//};
//esConnection.Reconnecting += (@event, _args) =>
//{
//    Log.Information("Reconnection occured");
//};
//esConnection.AuthenticationFailed += (@event, _args) =>
//{
//    Log.Information("Auth failed");
//};



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
