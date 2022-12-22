using Marketplace;
using Marketplace.ClassifiedAd;
using Marketplace.Domain.ClassifiedAd;
using Marketplace.Domain.Shared;
using Marketplace.Framework;
using Marketplace.Infrastructure;
using Raven.Client.Documents;
using Serilog;
using Marketplace.Domain.UserProfile;
using Marketplace.UserProfile;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var store = new DocumentStore
{
    Urls = new[] { "http://localhost:8080" },
    Database = "Marketplace_Chapter8",
    Conventions =
    {
        FindIdentityProperty = x => x.Name == "DbId"
    }

};
store.Initialize();

//const string connectionString = "Host=localhost;Database=Marketplace_Chapter8;Username=postgres;Password = kokikoki";
//builder.Services
//    .AddDbContext<ClassifiedAdDbContext>(
//    options => options.UseNpgsql(connectionString));

var profanityClient = new ProfanityClient();

Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger();
builder.Services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
builder.Services.AddScoped(c => store.OpenAsyncSession());

builder.Services.AddScoped<IUnitOfWork, RavenUnitOfWork>();

builder.Services.AddScoped<IClassifiedAdRepository, ClassifiedAddRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();

builder.Services.AddScoped<ClassifiedAdApplicationService>();
builder.Services.AddScoped(c =>
                        new UserProfileApplicationService(
                                c.GetRequiredService<IUserProfileRepository>(),
                                c.GetRequiredService<IUnitOfWork>(),
                                _text=>profanityClient.CheckForProfacity(_text)
                            ));

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