using FindFi.CL.Application.Abstractions.Repositories;
using FindFi.CL.Domain.Abstractions.Queries;
using FindFi.CL.Infrastructure.Configuration;
using FindFi.CL.Infrastructure.Persistence.Mongo.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureMongo(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind options
        var section = configuration.GetSection(MongoOptions.SectionName);
        services.Configure<MongoOptions>(section);

        // Register Mongo client and database
        services.AddSingleton<IMongoClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<MongoOptions>>().Value;
            var url = new MongoUrl(options.ConnectionString);
            var settings = MongoClientSettings.FromUrl(url);

            if (options.MaxPoolSize > 0) settings.MaxConnectionPoolSize = options.MaxPoolSize;
            if (options.MinPoolSize > 0) settings.MinConnectionPoolSize = options.MinPoolSize;
            if (options.ConnectTimeoutSeconds > 0) settings.ConnectTimeout = TimeSpan.FromSeconds(options.ConnectTimeoutSeconds);
            if (options.SocketTimeoutSeconds > 0) settings.SocketTimeout = TimeSpan.FromSeconds(options.SocketTimeoutSeconds);
            if (options.ServerSelectionTimeoutSeconds > 0) settings.ServerSelectionTimeout = TimeSpan.FromSeconds(options.ServerSelectionTimeoutSeconds);

            return new MongoClient(settings);
        });

        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<MongoOptions>>().Value;
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(options.DatabaseName);
        });

        // Repositories
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IReviewQueryRepository, ReviewQueryRepository>();
        services.AddScoped<IListingRatingRepository, ListingRatingRepository>();
        services.AddScoped<IThreadRepository, ThreadRepository>();
        services.AddScoped<IReactionRepository, ReactionRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();

        // Index creation + data seeding services
        services.AddSingleton<FindFi.CL.Infrastructure.Persistence.Mongo.Setup.IIndexCreationService, FindFi.CL.Infrastructure.Persistence.Mongo.Setup.MongoIndexCreationService>();
        services.AddScoped<FindFi.CL.Infrastructure.Persistence.Mongo.Seeding.IDataSeeder, FindFi.CL.Infrastructure.Persistence.Mongo.Seeding.ReviewsDataSeeder>();
        services.AddScoped<FindFi.CL.Infrastructure.Persistence.Mongo.Seeding.IDataSeeder, FindFi.CL.Infrastructure.Persistence.Mongo.Seeding.ListingRatingsDataSeeder>();
        services.AddScoped<FindFi.CL.Infrastructure.Persistence.Mongo.Seeding.IDataSeeder, FindFi.CL.Infrastructure.Persistence.Mongo.Seeding.ThreadsDataSeeder>();
        services.AddScoped<FindFi.CL.Infrastructure.Persistence.Mongo.Seeding.IDataSeeder, FindFi.CL.Infrastructure.Persistence.Mongo.Seeding.ReactionsDataSeeder>();
        services.AddScoped<FindFi.CL.Infrastructure.Persistence.Mongo.Seeding.IDataSeeder, FindFi.CL.Infrastructure.Persistence.Mongo.Seeding.ReportsDataSeeder>();

        return services;
    }
}
