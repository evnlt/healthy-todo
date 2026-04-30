using HealthyTodo.DAL.Entities;
using HealthyTodo.Models.TodoList;

namespace HealthyTodo.DAL.Extensions;

public static class TodoListEntityExtensions
{
    public static TodoListModel ToModel(this TodoListEntity entity)
    {
        return new TodoListModel
        {
            Id = entity.Id.ToString(),
            Title = entity.Title,
            CreatedDate = entity.CreatedDate,
            OwnerId = entity.OwnerId
        };
    }
}