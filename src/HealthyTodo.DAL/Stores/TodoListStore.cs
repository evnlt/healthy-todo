using HealthyTodo.Common;
using HealthyTodo.Common.Constants;
using HealthyTodo.DAL.Abstraction;
using HealthyTodo.DAL.Constants;
using HealthyTodo.DAL.Entities;
using HealthyTodo.DAL.Extensions;
using HealthyTodo.Models.TodoList;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HealthyTodo.DAL.Stores;

internal class TodoListStore : ITodoListStore
{
    private readonly IMongoDatabase _database;

    public TodoListStore(IMongoDatabase database)
    {
        _database = database;
    }

    // TODO - refactor this 
    public async Task<OffsetCollection<TodoListModel>> GetMany(
        TodoListFilter filter,
        OffsetPagination pager)
    {
        var seeder = new SampleDataSeeder(_database);
        await seeder.SeedAsync();
        
        var collection = GetCollection();
        var builder = Builders<TodoListEntity>.Filter;

        var filters = new List<FilterDefinition<TodoListEntity>>();

        if (filter.Ids is { Length: > 0 })
        {
            var objectIds = filter.Ids
                .Where(id => ObjectId.TryParse(id, out _))
                .Select(ObjectId.Parse)
                .ToList();

            if (objectIds.Count > 0)
            {
                filters.Add(builder.In(x => x.Id, objectIds));
            }
        }

        if (filter.UserId > 0)
        {
            var accessFilter = builder.Or(
                builder.Eq(x => x.OwnerId, filter.UserId),
                builder.AnyEq(x => x.UserIds, filter.UserId)
            );

            filters.Add(accessFilter);
        }

        var finalFilter = filters.Count > 0
            ? builder.And(filters)
            : builder.Empty;

        var totalCount = await collection.CountDocumentsAsync(finalFilter);

        var sortBuilder = Builders<TodoListEntity>.Sort;

        var sort = filter.OrderBy switch
        {
            TodoListOrderField.CreateDate => filter.OrderDirection == OrderDirection.Descending
                ? sortBuilder.Descending(x => x.CreatedDate)
                : sortBuilder.Ascending(x => x.CreatedDate),

            _ => sortBuilder.Descending(x => x.CreatedDate)
        };

        var items = await collection
            .Find(finalFilter)
            .Sort(sort)
            .Skip(pager.Offset)
            .Limit(pager.Take)
            .ToListAsync();

        return items
            .Select(x => x.ToModel())
            .ToOffsetCollection(pager.Offset, (int)totalCount);
    }

    public async Task<TodoListModel> Create(CreateTodoListModel model)
    {
        var collection = GetCollection();

        var entity = new TodoListEntity
        {
            Title = model.Title,
            OwnerId = model.OwnerId,
            CreatedDate = DateTime.UtcNow
        };

        await collection.InsertOneAsync(entity);

        return entity.ToModel();
    }

    public Task Update(UpdateTodoListModel model)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int todoListId)
    {
        throw new NotImplementedException();
    }

    private IMongoCollection<TodoListEntity> GetCollection()
    {
        return _database.GetCollection<TodoListEntity>(DataCollections.TodoLists);
    }

    // TODO - refactor
    private IMongoCollection<TodoListEntity> GetUserCollection()
    {
        return _database.GetCollection<TodoListEntity>(DataCollections.Users);
    }
}