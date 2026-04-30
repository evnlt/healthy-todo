using HealthyTodo.Common;
using HealthyTodo.Models.TodoList;

namespace HealthyTodo.BLL.Abstraction.Validators;

public interface ITodoListValidator
{
    Task Validate(TodoListFilter filter, OffsetPagination pager);
    Task Validate(CreateTodoListModel model);
    Task Validate(UpdateTodoListModel model);
    Task Validate(DeleteTodoListModel model);
}