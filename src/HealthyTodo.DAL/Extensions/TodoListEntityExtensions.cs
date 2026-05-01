using HealthyTodo.DAL.Entities;
using HealthyTodo.Models.TodoList;

namespace HealthyTodo.DAL.Extensions;

internal static class TodoListEntityExtensions
{
    public static TodoListModel ToModel(this TodoListEntity entity)
    {
        return new TodoListModel
        {
            Id = entity.Id,
            Title = entity.Title,
            CreatedDate = entity.CreatedDate,
            OwnerId = entity.OwnerId
        };
    }
    
    public static TodoListEntity ToEntity(this CreateTodoListModel entity)
    {
        return new TodoListEntity
        {
            Title = entity.Title,
            OwnerId = entity.OwnerId,
            CreatedDate = DateTime.UtcNow,
        };
    }
}