using FindFi.CL.Infrastructure;
using FindFi.CL.Application;
using FindFi.CL.WebAPI.Common.Middleware;
using AutoMapper;
using FindFi.CL.Infrastructure.Configuration;
using FindFi.CL.Infrastructure.Persistence.Mongo.Seeding;
using FindFi.CL.Infrastructure.Persistence.Mongo.Setup;
using Infrastructure;
using Microsoft.Extensions.Options;
using WebAPI.Health;
using WebAPI.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
// AutoMapper manual registration to avoid extension package dependency
var mapperConfig = new MapperConfiguration(cfg => cfg.AddMaps(typeof(WebApiMappingProfile).Assembly));
builder.Services.AddSingleton<IMapper>(sp => mapperConfig.CreateMapper());
builder.Services.AddHealthChecks()
    .AddCheck<MongoDbHealthCheck>("mongodb");

// Infrastructure (Mongo) DI registration according to Clean Architecture
builder.Services.AddInfrastructureMongo(builder.Configuration);
// Application (CQRS/MediatR)
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global exception handling (ProblemDetails + MongoDB-specific mappings)
app.UseGlobalExceptionHandling();

app.UseHttpsRedirection();
app.MapControllers();
app.MapHealthChecks("/health");

// Ensure indexes and optional data seeding on startup
using (var scope = app.Services.CreateScope())
{
    var indexer = scope.ServiceProvider.GetRequiredService<IIndexCreationService>();
    await indexer.CreateIndexesAsync();

    var cfg = scope.ServiceProvider.GetRequiredService<IOptions<MongoOptions>>().Value;
    if (cfg.Seed)
    {
        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync();
        }
    }
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}