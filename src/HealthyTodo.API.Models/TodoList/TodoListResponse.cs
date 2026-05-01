namespace HealthyTodo.API.Models.TodoList;

public class TodoListResponse
{
    public int Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
    
    public int OwnerId { get; set; }
}