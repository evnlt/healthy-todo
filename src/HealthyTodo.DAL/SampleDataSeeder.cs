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
            new() { Id = 3 },
            new() { Id = 4 },
            new() { Id = 5 },
            new() { Id = 6 },
            new() { Id = 7 },
            new() { Id = 8 },
            new() { Id = 9 },
            new() { Id = 10 },
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
            },
            new()
            {
                Title = "Work Tasks 2",
                CreatedDate = DateTime.UtcNow,
                OwnerId = 1,
                UserIds = [2, 3, 4]
            },
            new()
            {
                Title = "Personal 2",
                CreatedDate = DateTime.UtcNow,
                OwnerId = 4,
                UserIds = [1, 3, 7]
            },
            new()
            {
                Title = "Shared Project 2",
                CreatedDate = DateTime.UtcNow,
                OwnerId = 6,
                UserIds = [1, 2, 6, 7]
            },new()
            {
                Title = "Work Tasks 3",
                CreatedDate = DateTime.UtcNow,
                OwnerId = 4,
                UserIds = [2, 3]
            },
            new()
            {
                Title = "Personal 3",
                CreatedDate = DateTime.UtcNow,
                OwnerId = 8,
                UserIds = [1, 4, 8]
            },
            new()
            {
                Title = "Shared Project 3",
                CreatedDate = DateTime.UtcNow,
                OwnerId = 10,
                UserIds = [1, 2, 5, 7]
            },
        };

        await _todoLists.InsertManyAsync(todoLists);
    }
}