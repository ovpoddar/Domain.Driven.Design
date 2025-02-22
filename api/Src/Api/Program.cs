using DDD.Api.Extensions;
using DDD.Application;
using DDD.Infrastructure;
using DDD.Presentation;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.AddDatabaseLogging();

builder.Services
    .AddApiServicesConfiguration(configuration)
    .AddPresentation()
    .AddApplication(configuration)
    .AddInfrastructure(configuration);

builder.Services.AddErrorHandlingPipeLine();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors();
app.UseExceptionHandler();
app.MapControllers();
app.Run();
