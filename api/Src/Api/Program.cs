using DDD.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.AddDatabaseLogging(configuration);

builder.Services
    .AddApiServicesConfiguration(configuration)

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", () => "hellow");
app.Run();
