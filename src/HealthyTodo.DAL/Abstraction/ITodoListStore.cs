using HealthyTodo.Common;
using HealthyTodo.Models.TodoList;

namespace HealthyTodo.DAL.Abstraction;

public interface ITodoListStore
{
    Task<OffsetCollection<TodoListModel>> GetMany(TodoListFilter filter, OffsetPagination pager);
    Task<TodoListModel> Create(CreateTodoListModel model);
    Task Update(UpdateTodoListModel model);
    Task Delete(int todoListId);
    
    // create new todo list
    // update a todo list
    // delete a todo list
    // get one existing todo list
    // get many with filter and pagination
    
    // connect a todo list to a user
    // get user to which a certain todo list is connected
    // delete a connection between a certain user and todo list
}