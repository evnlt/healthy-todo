using HealthyTodo.Common.Constants;

namespace HealthyTodo.Models.TodoList;

public class TodoListFilter
{
    public int UserId { get; set; }
    public string[] Ids { get; set; } = [];
    public TodoListOrderField OrderBy { get; set; } = TodoListOrderField.CreateDate;
    public OrderDirection OrderDirection { get; set; }

}