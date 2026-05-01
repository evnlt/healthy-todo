using HealthyTodo.Common;
using HealthyTodo.Common.Constants;
using HealthyTodo.DAL.Abstraction;
using HealthyTodo.DAL.Constants;
using HealthyTodo.DAL.Entities;
using HealthyTodo.DAL.Extensions;
using HealthyTodo.Models.TodoList;
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
        var collection = GetCollection();
        var builder = Builders<TodoListEntity>.Filter;

        var filters = new List<FilterDefinition<TodoListEntity>>();

        if (filter.Ids.Length > 0)
        {
            filters.Add(builder.In(x => x.Id, filter.Ids));
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

        var result = items
            .Select(x => x.ToModel())
            .ToOffsetCollection(pager.Offset, (int)totalCount);

        return result;
    }

    public async Task<TodoListModel?> GetById(int id)
    {
        var collection = GetCollection();

        var entity = await collection
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync();

        return entity?.ToModel();
    }

    public async Task<TodoListModel> Create(CreateTodoListModel model)
    {
        var collection = GetCollection();
        
        var entity = model.ToEntity();
        
        await collection.InsertOneAsync(entity);
        
        return entity.ToModel();
    }

    public async Task<TodoListModel> Update(UpdateTodoListModel model)
    {
        var collection = GetCollection();

        var update = Builders<TodoListEntity>.Update
            .Set(x => x.Title, model.Title);

        TodoListEntity updatedEntity = await collection.FindOneAndUpdateAsync(
            x => x.Id == model.Id,
            update,
            new FindOneAndUpdateOptions<TodoListEntity>
            {
                ReturnDocument = ReturnDocument.After
            }
        );

        return updatedEntity.ToModel();
    }

    public async Task Delete(int todoListId)
    {
        var collection = GetCollection();

        await collection.DeleteOneAsync(x => x.Id == todoListId);
    }

    public async Task UpdateUsers(int listId, List<int> userIds)
    {
        var collection = GetCollection();

        var update = Builders<TodoListEntity>.Update
            .Set(x => x.UserIds, userIds);

        await collection.UpdateOneAsync(
            x => x.Id == listId,
            update
        );
    }

    public async Task AddUser(int listId, int userId)
    {
        var collection = GetCollection();

        var update = Builders<TodoListEntity>.Update
            .AddToSet(x => x.UserIds, userId);

        await collection.UpdateOneAsync(
            x => x.Id == listId,
            update
        );
    }

    public async Task RemoveUser(int listId, int userId)
    {
        var collection = GetCollection();

        var update = Builders<TodoListEntity>.Update
            .Pull(x => x.UserIds, userId);

        await collection.UpdateOneAsync(
            x => x.Id == listId,
            update
        );
    }

    private IMongoCollection<TodoListEntity> GetCollection()
    {
        return _database.GetCollection<TodoListEntity>(DataCollections.TodoLists);
    }
}