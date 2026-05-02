using System.ComponentModel.DataAnnotations;
using HealthyTodo.API.Models.Common;
using HealthyTodo.Common.Constants;

namespace HealthyTodo.API.Models.TodoList;

public class TodoListFilterRequest : FilterBaseRequest<TodoListOrderField>
{
    public string[] Ids { get; set; } = [];
    
    [Required]
    public int UserId { get; set; }
}