using HealthyTodo.DAL.Constants;
using HealthyTodo.DAL.Entities;
using MongoDB.Driver;

namespace HealthyTodo.DAL;

public class MongoInitializer
{
    private readonly IMongoDatabase _database;

    public MongoInitializer(IMongoDatabase database)
    {
        _database = database;
    }

    // basically a migration
    public async Task InitializeAsync()
    {
        var collection = _database.GetCollection<TodoListEntity>(DataCollections.TodoLists);

        var indexes = new[]
        {
            new CreateIndexModel<TodoListEntity>(
                Builders<TodoListEntity>.IndexKeys.Ascending(x => x.OwnerId)),

            new CreateIndexModel<TodoListEntity>(
                Builders<TodoListEntity>.IndexKeys.Ascending(x => x.UserIds)),

            new CreateIndexModel<TodoListEntity>(
                Builders<TodoListEntity>.IndexKeys.Descending(x => x.CreatedDate))
        };

        await collection.Indexes.CreateManyAsync(indexes);
    }
}