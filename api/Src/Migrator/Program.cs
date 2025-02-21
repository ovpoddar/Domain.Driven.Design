using DDD.Infrastructure;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();
builder.Services.AddInfrastructure(builder.Configuration, typeof(DDD.Migrator.IAssemblyMarker).Assembly.GetName().Name);

var app = builder.Build();

