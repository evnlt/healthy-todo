using HealthyTodo.Common;
using HealthyTodo.Models.TodoList;

namespace HealthyTodo.BLL.Abstraction.Services;

public interface ITodoListService
{
    Task<OffsetCollection<TodoListModel>> GetMany(TodoListFilter filter, OffsetPagination pager);
    Task<TodoListModel> Create(CreateTodoListModel model);
    Task<TodoListModel> Update(UpdateTodoListModel model);
    Task Delete(DeleteTodoListModel model);
}