namespace HealthyTodo.API.Models.TodoList;

public class CreateTodoListRequest
{
    public string Title { get; set; } = default!;
    public int OwnerId { get; set; }
}