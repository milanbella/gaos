#pragma warning disable 8600, 8602, 8604

using Microsoft.EntityFrameworkCore;
using Gaos.Dbo;
using Gaos.Routes;
using Serilog;
using Gaos.Middleware;

var dbConnectionString = "server=localhost;user=root;password=root;database=gaos";
var dbServerVersion = new MariaDbServerVersion(new Version(10, 7));


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Db>(opt => 
    opt.UseMySql(dbConnectionString, dbServerVersion)
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddScoped<Gaos.Common.Guest>(provider => {
    Db db = provider.GetService<Db>();
    string guestNamesFilePath = "py/guest_names.txt";
    return new Gaos.Common.Guest(db, guestNamesFilePath);
}); 

builder.Services.AddScoped<Gaos.Auth.Token>(provider => {
    Db db = provider.GetService<Db>();
    string pkcs12KeyStoreFilePath = "/w1/gaos/scripts/out/key_store.pfx";
    string keyStorePassword = "changeit";
    return new Gaos.Auth.Token(pkcs12KeyStoreFilePath, keyStorePassword, db);
}); 


var app = builder.Build();

app.UseMiddleware<AuthMiddleware>();


app.Map("/", () => Results.Ok("hello!"));
app.MapGroup("/todoitems").GroupTodosItems();
app.MapGroup("/user").GroupUser();
app.MapGroup("/device").GroupDevice();
app.MapGroup("/api").GroupApi();


app.Run();