using HealthyTodo.API.Extensions;
using HealthyTodo.API.Models.Common.Extensions;
using HealthyTodo.API.Models.TodoList;
using HealthyTodo.BLL.Abstraction.Services;
using HealthyTodo.Common;
using HealthyTodo.Models.TodoList;
using Microsoft.AspNetCore.Mvc;

namespace HealthyTodo.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/todolists")]
public class TodoListController : ControllerBase
{
    // create new todo list
    // update a todo list
    // delete a todo list
    // get one existing todo list
    // get many with filter and pagination

    // connect a todo list to a user
    // get user to which a certain todo list is connected
    // delete a connection between a certain user and todo list


    private readonly ITodoListService _todoListService;

    public TodoListController(ITodoListService todoListService)
    {
        _todoListService = todoListService;
    }

    [HttpGet("")]
    public async Task<List<TodoListResponse>> GetMany([FromQuery] TodoListRequest request)
    {
        TodoListFilter filter = request.ToFilter();
        OffsetPagination pager = request.ToPager();

        OffsetCollection<TodoListModel> offsetCollection = await _todoListService.GetMany(filter, pager);
        //this.AddPaginationHeaders(offsetCollection); // TODO - ?
        List<TodoListResponse> response = offsetCollection.Select(_ => _.ToResponse()).ToList();
        return response;
    }

    [HttpGet("{id}")]
    public Task<IActionResult> GetDetails([FromRoute] int id) // TODO - implement?
    {
        throw new NotImplementedException();
    }

    [HttpPost("")]
    public async Task<IActionResult> Create(CreateTodoListRequest request)
    {
        CreateTodoListModel model = request.ToModel();
        TodoListModel todoList = await _todoListService.Create(model);

        return CreatedAtAction(nameof(GetDetails), new { id = todoList.Id }, todoList.ToResponse());
    }
}