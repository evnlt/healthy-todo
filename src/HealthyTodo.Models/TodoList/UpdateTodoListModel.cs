namespace HealthyTodo.Models.TodoList;

public class UpdateTodoListModel
{
    public int Id { get; set; } = default!;
    public string Title { get; set; } = default!;

    public int UserId { get; set; }
}