using HealthyTodo.BLL.Abstraction.Services;
using HealthyTodo.BLL.Abstraction.Validators;
using HealthyTodo.BLL.Exceptions;
using HealthyTodo.Common;
using HealthyTodo.DAL.Abstraction;
using HealthyTodo.Models.TodoList;
using Microsoft.Extensions.Logging;

namespace HealthyTodo.BLL.Services;

internal class TodoListService : ITodoListService
{
    private readonly ITodoListStore _todoListStore;
    private readonly ITodoListValidator _todoListValidator;
    private readonly ILogger<TodoListService> _logger;

    public TodoListService(
        ITodoListStore todoListStore, 
        ITodoListValidator todoListValidator,
        ILogger<TodoListService> logger)
    {
        _todoListStore = todoListStore;
        _todoListValidator = todoListValidator;
        _logger = logger;
    }

    public async Task<OffsetCollection<TodoListModel>> GetMany(TodoListFilter filter, OffsetPagination pager)
    {
        await _todoListValidator.Validate(filter, pager);
        
        _logger.LogInformation(
            "Getting todo lists for user {UserId} with offset {Offset} and take {Take}",
            filter.UserId, pager.Offset, pager.Take);

        return await _todoListStore.GetMany(filter, pager);
    }

    public async Task<TodoListModel> GetById(string listId, int userId)
    {
        var list = await _todoListStore.GetById(listId);

        if (list == null)
        {
            _logger.LogWarning("Todo list {ListId} not found", listId);
            throw new NotFoundException(ErrorMessages.TodoListNotFound);
        }

        if (!HasAccess(list, userId))
        {
            _logger.LogWarning("Access denied to list {ListId} for user {UserId}", listId, userId);
            throw new ForbiddenException(ErrorMessages.AccessDenied);
        }
        
        _logger.LogInformation("Getting todo list {ListId} for user {UserId}", listId, userId);

        return list;
    }

    public async Task<TodoListModel> Create(CreateTodoListModel model)
    {
        await _todoListValidator.Validate(model);
        
        _logger.LogInformation("Creating todo list for user {UserId}", model.OwnerId);

        return await _todoListStore.Create(model);
    }

    public async Task<TodoListModel> Update(UpdateTodoListModel model)
    {
        await _todoListValidator.Validate(model);
        
        _logger.LogInformation("Updating todo list {ListId} by user {UserId}", model.Id, model.UserId);

        var existing = await _todoListStore.GetById(model.Id);

        if (existing == null)
        {
            _logger.LogWarning("Todo list {ListId} not found for update", model.Id);
            throw new NotFoundException(ErrorMessages.TodoListNotFound);
        }

        if (!HasAccess(existing, model.UserId))
        {
            _logger.LogWarning("Access denied to update list {ListId} for user {UserId}", model.Id, model.UserId);
            throw new ForbiddenException(ErrorMessages.AccessDenied);
        }

        return await _todoListStore.Update(model);
    }

    public async Task<bool> Delete(string listId, int userId)
    {
        _logger.LogInformation("Deleting todo list {ListId} by user {UserId}", listId, userId);
        
        var existing = await _todoListStore.GetById(listId);

        if (existing == null)
        {
            _logger.LogWarning("Todo list {ListId} not found for deletion", listId);
            throw new NotFoundException(ErrorMessages.TodoListNotFound);
        }

        if (!IsOwner(existing, userId))
        {
            _logger.LogWarning("User {UserId} attempted to delete list {ListId} without ownership", userId, listId);
            throw new ForbiddenException(ErrorMessages.OnlyOwnerCanDelete);
        }

        await _todoListStore.Delete(listId);

        return true;
    }

    public async Task<bool> AddUser(string listId, int userId, int targetUserId)
    {
        _logger.LogInformation(
            "User {UserId} adding user {TargetUserId} to list {ListId}",
            userId, targetUserId, listId);
        
        var list = await _todoListStore.GetById(listId);

        if (list == null)
        {
            _logger.LogWarning("Todo list {ListId} not found when adding user", listId);
            throw new NotFoundException(ErrorMessages.TodoListNotFound);
        }

        if (!HasAccess(list, userId))
        {
            _logger.LogWarning("Access denied to list {ListId} for user {UserId}", listId, userId);
            throw new ForbiddenException(ErrorMessages.AccessDenied);
        }

        if (list.OwnerId == targetUserId)
        {
            _logger.LogWarning(
                "User {UserId} attempted to add owner {TargetUserId} to list {ListId}",
                userId, targetUserId, listId);
            
            throw new BadRequestException(ErrorMessages.CannotAddOwner);
        }

        if (list.SharedUserIds.Contains(targetUserId))
        {
            _logger.LogWarning(
                "User {UserId} attempted to remove user {TargetUserId} from list {ListId}, but the user is not part of the list",
                userId, targetUserId, listId);
            
            throw new BadRequestException(ErrorMessages.UserAlreadyAdded);
        }

        list.SharedUserIds.Add(targetUserId);

        await _todoListStore.UpdateUsers(listId, list.SharedUserIds);

        return true;
    }

    public async Task<List<int>> GetUsers(string listId, int userId)
    {
        _logger.LogInformation("Getting users for list {ListId} by user {UserId}", listId, userId);
        
        var list = await _todoListStore.GetById(listId);

        if (list == null)
        {
            _logger.LogWarning("Todo list {ListId} not found when getting users", listId);
            throw new NotFoundException(ErrorMessages.TodoListNotFound);
        }

        if (!HasAccess(list, userId))
        {
            _logger.LogWarning("Access denied to list {ListId} for user {UserId}", listId, userId);
            throw new ForbiddenException(ErrorMessages.AccessDenied);
        }

        return list.SharedUserIds;
    }

    public async Task<bool> RemoveUser(string listId, int userId, int targetUserId)
    {
        _logger.LogInformation(
            "User {UserId} removing user {TargetUserId} from list {ListId}",
            userId, targetUserId, listId);
        
        var list = await _todoListStore.GetById(listId);

        if (list == null)
        {
            _logger.LogWarning("Todo list {ListId} not found when removing user", listId);
            throw new NotFoundException(ErrorMessages.TodoListNotFound);
        }

        if (!HasAccess(list, userId))
        {
            _logger.LogWarning("Access denied to list {ListId} for user {UserId}", listId, userId);
            throw new ForbiddenException(ErrorMessages.AccessDenied);
        }

        if (list.OwnerId == targetUserId)
        {
            _logger.LogWarning(
                "User {UserId} attempted to add owner {TargetUserId} to list {ListId}",
                userId, targetUserId, listId);
            
            throw new BadRequestException(ErrorMessages.CannotRemoveOwner);
        }

        if (!list.SharedUserIds.Contains(targetUserId))
        {
            _logger.LogWarning(
                "User {UserId} attempted to remove user {TargetUserId} from list {ListId}, but the user is not part of the list",
                userId, targetUserId, listId);
            
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