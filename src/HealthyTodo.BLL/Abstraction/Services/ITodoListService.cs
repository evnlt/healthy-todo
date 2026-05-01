using HealthyTodo.Common;
using HealthyTodo.Models.TodoList;

namespace HealthyTodo.BLL.Abstraction.Services;

public interface ITodoListService
{
    Task<OffsetCollection<TodoListModel>> GetMany(TodoListFilter filter, OffsetPagination pager);
    Task<TodoListModel> GetById(int id, int userId);
    Task<TodoListModel> Create(CreateTodoListModel model);
    Task<TodoListModel> Update(UpdateTodoListModel model);
    Task<bool> Delete(int id, int userId);
    Task<bool> AddUser(int listId, int userId, int targetUserId);
    Task<List<int>> GetUsers(int listId, int userId);
    Task<bool> RemoveUser(int listId, int userId, int targetUserId);
}