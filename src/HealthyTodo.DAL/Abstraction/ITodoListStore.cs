using HealthyTodo.Common;
using HealthyTodo.Models.TodoList;

namespace HealthyTodo.DAL.Abstraction;

public interface ITodoListStore
{
    Task<OffsetCollection<TodoListModel>> GetMany(TodoListFilter filter, OffsetPagination pager);
    Task<TodoListModel?> GetById(string id);
    Task<TodoListModel> Create(CreateTodoListModel model);
    Task<TodoListModel> Update(UpdateTodoListModel model);
    Task Delete(string todoListId);
    Task UpdateUsers(string listId, List<int> userIds);
    Task AddUser(string listId, int userId);
    Task RemoveUser(string listId, int userId);
}