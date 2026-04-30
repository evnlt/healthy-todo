using HealthyTodo.BLL.Abstraction.Validators;
using HealthyTodo.Common;
using HealthyTodo.Models.TodoList;

namespace HealthyTodo.BLL.Validators;

public class TodoListValidator : ITodoListValidator
{
    public Task Validate(TodoListFilter filter, OffsetPagination pager)
    {
        // TODO - IMPLEMENT!!!!
        return Task.CompletedTask;
    }

    public Task Validate(CreateTodoListModel model)
    {
        throw new NotImplementedException();
    }

    public Task Validate(UpdateTodoListModel model)
    {
        throw new NotImplementedException();
    }

    public Task Validate(DeleteTodoListModel model)
    {
        throw new NotImplementedException();
    }
}