namespace HealthyTodo.Models.TodoList;

public class CreateTodoListModel
{
    public string Title { get; set; } = default!;
    public int OwnerId { get; set; }
}