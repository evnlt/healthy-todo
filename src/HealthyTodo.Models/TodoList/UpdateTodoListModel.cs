namespace HealthyTodo.Models.TodoList;

public class UpdateTodoListModel
{
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;

    public int UserId { get; set; }
}