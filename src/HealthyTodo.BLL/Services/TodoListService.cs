using HealthyTodo.BLL.Abstraction.Services;
using HealthyTodo.BLL.Abstraction.Validators;
using HealthyTodo.Common;
using HealthyTodo.DAL.Abstraction;
using HealthyTodo.Models.TodoList;

namespace HealthyTodo.BLL.Services;

public class TodoListService : ITodoListService
{
    private readonly ITodoListStore _todoListStore;
    private readonly ITodoListValidator _todoListValidator;

    public TodoListService(ITodoListStore todoListStore, ITodoListValidator todoListValidator)
    {
        _todoListStore = todoListStore;
        _todoListValidator = todoListValidator;
    }

    public async Task<OffsetCollection<TodoListModel>> GetMany(TodoListFilter filter, OffsetPagination pager)
    {
        await _todoListValidator.Validate(filter, pager);

        return await _todoListStore.GetMany(filter, pager);
    }

    public async Task<TodoListModel> Create(CreateTodoListModel model)
    {
        await _todoListValidator.Validate(model);

        return await _todoListStore.Create(model);
    }

    public Task<TodoListModel> Update(UpdateTodoListModel model)
    {
        throw new NotImplementedException();
    }

    public Task Delete(DeleteTodoListModel model)
    {
        throw new NotImplementedException();
    }
}