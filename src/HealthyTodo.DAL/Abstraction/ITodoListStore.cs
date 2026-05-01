using HealthyTodo.Common;
using HealthyTodo.Models.TodoList;

namespace HealthyTodo.DAL.Abstraction;

public interface ITodoListStore
{
    Task<OffsetCollection<TodoListModel>> GetMany(TodoListFilter filter, OffsetPagination pager);
    Task<TodoListModel?> GetById(int id);
    Task<TodoListModel> Create(CreateTodoListModel model);
    Task<TodoListModel> Update(UpdateTodoListModel model);
    Task Delete(int todoListId);
    Task UpdateUsers(int listId, List<int> userIds);
    Task AddUser(int listId, int userId);
    Task RemoveUser(int listId, int userId);
}