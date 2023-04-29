using Microsoft.EntityFrameworkCore;
using gaos.Dbo;
using gaos.Routes;
using Serilog;
using gaos.Middleware;

var dbConnectionString = "server=localhost;user=root;password=root;database=usrv";
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


var app = builder.Build();

app.UseMiddleware<AuthMiddleware>();


app.Map("/", () => Results.Ok("hello!"));
app.MapGroup("/todoitems").GroupTodosItems();
app.MapGroup("/user").GroupUser();
app.MapGroup("/api").GroupApi();


app.Run();