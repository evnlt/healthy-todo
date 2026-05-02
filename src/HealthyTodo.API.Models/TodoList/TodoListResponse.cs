namespace HealthyTodo.API.Models.TodoList;

public class TodoListResponse
{
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
    
    public int OwnerId { get; set; }
}