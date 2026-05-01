using HealthyTodo.API.Models.TodoList;
using HealthyTodo.Models.TodoList;

namespace HealthyTodo.API.Extensions;

internal static class TodoListExtensions
{
    public static TodoListResponse ToResponse(this TodoListModel model) // TODO - unit test
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
    
    public static CreateTodoListModel ToModel(this CreateTodoListRequest model) // TODO - unit test
    {
        var responseModel = new CreateTodoListModel
        {
            Title = model.Title,
            OwnerId = model.OwnerId
        };
        
        return responseModel;
    }
    
    public static UpdateTodoListModel ToModel(this UpdateTodoListRequest model, int id, int userId) // TODO - unit test
    {
        var responseModel = new UpdateTodoListModel
        {
            Id = id,
            UserId = userId,
            Title = model.Title,
        };
        
        return responseModel;
    }
    
    public static TodoListFilter ToFilter(this TodoListFilterRequest filterRequest) // TODO - unit test
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