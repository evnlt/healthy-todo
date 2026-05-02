using HealthyTodo.Common;
using HealthyTodo.Models.TodoList;

namespace HealthyTodo.BLL.Abstraction.Services;

public interface ITodoListService
{
    Task<OffsetCollection<TodoListModel>> GetMany(TodoListFilter filter, OffsetPagination pager);
    Task<TodoListModel> GetById(string listId, int userId);
    Task<TodoListModel> Create(CreateTodoListModel model);
    Task<TodoListModel> Update(UpdateTodoListModel model);
    Task<bool> Delete(string listId, int userId);
    Task<bool> AddUser(string listId, int userId, int targetUserId);
    Task<List<int>> GetUsers(string listId, int userId);
    Task<bool> RemoveUser(string listId, int userId, int targetUserId);
}