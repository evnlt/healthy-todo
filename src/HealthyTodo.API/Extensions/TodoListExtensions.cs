using HealthyTodo.API.Models.TodoList;
using HealthyTodo.Models.TodoList;

namespace HealthyTodo.API.Extensions;

internal static class TodoListExtensions
{
    public static TodoListResponse ToResponse(this TodoListModel model)
    {
        var responseModel = new TodoListResponse
        {
            Id = model.Id,
            Title = model.Title,
            CreatedDate = model.CreatedDate,
            OwnerId = model.OwnerId
        };
        
        return responseModel;
    }
    
    public static CreateTodoListModel ToModel(this CreateTodoListRequest request)
    {
        var responseModel = new CreateTodoListModel
        {
            Title = request.Title,
            OwnerId = request.OwnerId
        };
        
        return responseModel;
    }
    
    public static UpdateTodoListModel ToModel(this UpdateTodoListRequest request, string listId, int userId)
    {
        var responseModel = new UpdateTodoListModel
        {
            Id = listId,
            UserId = userId,
            Title = request.Title,
        };
        
        return responseModel;
    }
    
    public static TodoListFilter ToFilter(this TodoListFilterRequest filterRequest)
    {
        var filter = new TodoListFilter
        {
            Ids = filterRequest.Ids,
            UserId = filterRequest.UserId,
            OrderBy = filterRequest.OrderBy,
            OrderDirection = filterRequest.OrderDirection
        };

        return filter;
    }
}