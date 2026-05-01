namespace HealthyTodo.DAL.Entities;

internal class TodoListEntity
{
    public int Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateTime CreatedDate { get; set; }

    public int OwnerId { get; set; } // owns the todo list (rights: everything AND delete)
    public List<int> UserIds { get; set; } = new(); // shared users, have access to this todo list (rights: everything EXCEPT delete)*/
}