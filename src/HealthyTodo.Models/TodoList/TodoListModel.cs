namespace HealthyTodo.Models.TodoList;

public class TodoListModel
{
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
    
    public int OwnerId { get; set; }
    public List<int> SharedUserIds { get; set; } = [];
}