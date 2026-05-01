using HealthyTodo.DAL.Abstraction;
using HealthyTodo.DAL.Settings;
using HealthyTodo.DAL.Stores;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace HealthyTodo.DAL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, DatabaseSettings settings)
    {
        MongoClientSettings clientSettings = MongoClientSettings.FromUrl(new MongoUrl(settings.ConnectionString));

        services.AddSingleton<IMongoClient>(_ =>
        {
#if DEBUG
            clientSettings.LoggingSettings = new LoggingSettings(_.GetService<ILoggerFactory>());
#endif
            return new MongoClient(clientSettings);
        });
        services.AddTransient<IMongoDatabase>(provider => provider.GetService<IMongoClient>()
            !.GetDatabase(settings.DatabaseName));

        services
            .AddTransient<ITodoListStore, TodoListStore>()
            .AddTransient<IUserStore, UserStore>();
        
        services
            .AddScoped<SampleDataSeeder>();

        return services;
    }
}