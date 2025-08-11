using RustRetail.CatalogService.API.Configuration;
using RustRetail.CatalogService.Application;
using RustRetail.CatalogService.Infrastructure;
using RustRetail.CatalogService.Persistence;
using RustRetail.SharedInfrastructure.Logging.Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApplicationOptions(builder.Configuration)
    .AddBuiltInServices()
    .AddPersistence()
    .AddInfrastructure()
    .AddApplication()
    .AddApi();
builder.Host.UseSharedSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.ConfigureApplicationPipeline();

app.Run();
