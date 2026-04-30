using HealthyTodo.API.Models.TodoList;
using HealthyTodo.Models.TodoList;

namespace HealthyTodo.API.Extensions;

internal static class TodoListExtensions
{
    public static TodoListResponse ToResponse(this TodoListModel model) // TODO - unit test
    {
        var responseModel = new TodoListResponse
        {
        };
        
        return responseModel;
    }
    
    public static CreateTodoListModel ToModel(this CreateTodoListRequest model) // TODO - unit test
    {
        var responseModel = new CreateTodoListModel
        {
        };
        
        return responseModel;
    }
    
    public static TodoListFilter ToFilter(this TodoListRequest request) // TODO - unit test
    {
        var filter = new TodoListFilter
        {
        };

        return filter;
    }
}