using DDD.Infrastructure;
using DDD.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();
builder.Services.AddInfrastructure(builder.Configuration, typeof(DDD.Migrator.IAssemblyMarker).Assembly);
var app = builder.Build();

var applicationDbContext = app.Services.GetRequiredService<ApplicationDbContext>();
try
{
    await applicationDbContext.Database
        .MigrateAsync();
    Console.WriteLine("Migration Completed.");

    CancellationTokenSource tokenSource = new();
    await applicationDbContext.Database.EnsureCreatedAsync(tokenSource.Token);
    Console.WriteLine("Database Seeding SuccessFull.");
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
Console.WriteLine("Press ENTER to exit...");
Console.ReadLine();
