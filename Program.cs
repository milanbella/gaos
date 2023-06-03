#pragma warning disable 8600, 8602, 8604

using Microsoft.EntityFrameworkCore;
using Gaos.Dbo;
using Gaos.Routes;
using Serilog;
using Gaos.Middleware;
using System.Diagnostics;
using System.Net.WebSockets;



var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
//builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();


if (builder.Configuration["db_connection_string"] == null)
{
    throw new Exception("missing configuration value: db_connection_string");

}
var dbConnectionString = builder.Configuration.GetValue<string>("db_connection_string");

if (builder.Configuration["db_major_version"] == null)
{
    throw new Exception("missing configuration value: db_major_version");
}
var dbMajorVersion = builder.Configuration.GetValue<int>("db_major_version");

if (builder.Configuration["db_minor_version"] == null)
{
    throw new Exception("missing configuration value: db_minor_version");
}
var dbMinorVersion = builder.Configuration.GetValue<int>("db_minor_version");


var dbServerVersion = new MariaDbServerVersion(new Version(dbMajorVersion, dbMinorVersion));


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

if (builder.Configuration["guest_names_file_path"] == null)
{
    throw new Exception("missing configuration value: guest_names_file_path");
}
string guestNamesFilePath = builder.Configuration.GetValue<string>("guest_names_file_path");

builder.Services.AddScoped<Gaos.Common.Guest>(provider => {
    Db db = provider.GetService<Db>();
    return new Gaos.Common.Guest(db, guestNamesFilePath);
}); 


builder.Services.AddScoped<Gaos.Auth.Token>(provider => {
    Db db = provider.GetService<Db>();
    return new Gaos.Auth.Token(builder.Configuration,  db);
}); 


var app = builder.Build();

if (false) { 
    app.UseWebSockets();
    app.UseMiddleware<WebSocketMiddleware>();
}
app.UseMiddleware<AuthMiddleware>();


app.Map("/", (IConfiguration configuration) => {
    Console.WriteLine($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 200: db_connection_string:  {configuration["db_connection_string"]}");
    return Results.Ok("hello!");
});
app.MapGroup("/todoitems").GroupTodosItems();
app.MapGroup("/user").GroupUser();
app.MapGroup("/device").GroupDevice();
app.MapGroup("/api").GroupApi();


app.Run();