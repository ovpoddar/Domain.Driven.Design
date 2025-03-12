using DDD.Api.Extensions;
using DDD.Application;
using DDD.Infrastructure;
using DDD.Presentation;
using Microsoft.AspNetCore.Identity;

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

app.UseAuthorization();
app.MapIdentityApi<IdentityUser>();
app.UseCors();
app.UseExceptionHandler();
app.MapControllers();
app.UseHubs();
app.Run();
