using HealthyTodo.API.Models.Common;
using HealthyTodo.Common.Constants;

namespace HealthyTodo.API.Models.TodoList;

public class TodoListFilterRequest : FilterBaseRequest<TodoListOrderField>
{
    public int[] Ids { get; set; } = [];
    public int UserId { get; set; }
}