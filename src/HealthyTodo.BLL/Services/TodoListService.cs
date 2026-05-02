using HealthyTodo.BLL.Abstraction.Services;
using HealthyTodo.BLL.Abstraction.Validators;
using HealthyTodo.BLL.Exceptions;
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

    public async Task<TodoListModel> GetById(string listId, int userId)
    {
        var list = await _todoListStore.GetById(listId);

        if (list == null)
        {
            throw new NotFoundException(ErrorMessages.TodoListNotFound);
        }

        if (!HasAccess(list, userId))
        {
            throw new ForbiddenException(ErrorMessages.AccessDenied);
        }

        return list;
    }

    public async Task<TodoListModel> Create(CreateTodoListModel model)
    {
        await _todoListValidator.Validate(model);

        return await _todoListStore.Create(model);
    }

    public async Task<TodoListModel> Update(UpdateTodoListModel model)
    {
        await _todoListValidator.Validate(model);

        var existing = await _todoListStore.GetById(model.Id);

        if (existing == null)
        {
            throw new NotFoundException(ErrorMessages.TodoListNotFound);
        }

        if (!HasAccess(existing, model.UserId))
        {
            throw new ForbiddenException(ErrorMessages.AccessDenied);
        }

        return await _todoListStore.Update(model);
    }

    public async Task<bool> Delete(string listId, int userId)
    {
        var existing = await _todoListStore.GetById(listId);

        if (existing == null)
        {
            throw new NotFoundException(ErrorMessages.TodoListNotFound);
        }

        if (!IsOwner(existing, userId))
        {
            throw new ForbiddenException(ErrorMessages.OnlyOwnerCanDelete);
        }

        await _todoListStore.Delete(listId);

        return true;
    }

    public async Task<bool> AddUser(string listId, int userId, int targetUserId)
    {
        var list = await _todoListStore.GetById(listId);

        if (list == null)
        {
            throw new NotFoundException(ErrorMessages.TodoListNotFound);
        }

        if (!HasAccess(list, userId))
        {
            throw new ForbiddenException(ErrorMessages.AccessDenied);
        }

        if (list.OwnerId == targetUserId)
        {
            throw new BadRequestException(ErrorMessages.CannotAddOwner);
        }

        if (list.SharedUserIds.Contains(targetUserId))
        {
            throw new BadRequestException(ErrorMessages.UserAlreadyAdded);
        }

        list.SharedUserIds.Add(targetUserId);

        await _todoListStore.UpdateUsers(listId, list.SharedUserIds);

        return true;
    }

    public async Task<List<int>> GetUsers(string listId, int userId)
    {
        var list = await _todoListStore.GetById(listId);

        if (list == null)
        {
            throw new NotFoundException(ErrorMessages.TodoListNotFound);
        }

        if (!HasAccess(list, userId))
        {
            throw new ForbiddenException(ErrorMessages.AccessDenied);
        }

        return list.SharedUserIds;
    }

    public async Task<bool> RemoveUser(string listId, int userId, int targetUserId)
    {
        var list = await _todoListStore.GetById(listId);

        if (list == null)
        {
            throw new NotFoundException(ErrorMessages.TodoListNotFound);
        }

        if (!HasAccess(list, userId))
        {
            throw new ForbiddenException(ErrorMessages.AccessDenied);
        }

        if (list.OwnerId == targetUserId)
        {
            throw new BadRequestException(ErrorMessages.CannotRemoveOwner);
        }

        if (!list.SharedUserIds.Contains(targetUserId))
        {
            throw new BadRequestException(ErrorMessages.UserNotInList);
        }

        list.SharedUserIds.Remove(targetUserId);

        await _todoListStore.UpdateUsers(listId, list.SharedUserIds);

        return true;
    }

    #region private

    private static bool HasAccess(TodoListModel list, int userId)
    {
        return list.OwnerId == userId || list.SharedUserIds.Contains(userId);
    }

    private static bool IsOwner(TodoListModel list, int userId)
    {
        return list.OwnerId == userId;
    }

    #endregion
}