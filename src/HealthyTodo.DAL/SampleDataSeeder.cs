using HealthyTodo.DAL.Constants;
using HealthyTodo.DAL.Entities;
using MongoDB.Driver;

namespace HealthyTodo.DAL;

public class SampleDataSeeder
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<UserEntity> _users;
    private readonly IMongoCollection<TodoListEntity> _todoLists;

    public SampleDataSeeder(IMongoDatabase database)
    {
        _database = database;
        _users = database.GetCollection<UserEntity>(DataCollections.Users);
        _todoLists = database.GetCollection<TodoListEntity>(DataCollections.TodoLists);
    }

    public async Task SeedAsync()
    {
        var usersExist = await _users.CountDocumentsAsync(_ => true);
        if (usersExist > 0)
        {
            return;
        }

        var users = new List<UserEntity>
        {
            new() { Id = 1 },
            new() { Id = 2 },
            new() { Id = 3 }
        };

        await _users.InsertManyAsync(users);

        var todoLists = new List<TodoListEntity>
        {
            new()
            {
                Title = "Work Tasks",
                CreatedDate = DateTime.UtcNow,
                OwnerId = 1,
                UserIds = [2, 3]
            },
            new()
            {
                Title = "Personal",
                CreatedDate = DateTime.UtcNow,
                OwnerId = 2,
                UserIds = [1]
            },
            new()
            {
                Title = "Shared Project",
                CreatedDate = DateTime.UtcNow,
                OwnerId = 3,
                UserIds = [1, 2]
            }
        };

        await _todoLists.InsertManyAsync(todoLists);
    }
}