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
    // TODO - remove use cases?
    // get one existing todo list (owner or shared)
    // get many with filter and pagination (owner or shared)
    // create new todo list
    // update a todo list (owner or shared)
    // delete a todo list (only the owner)

    // share a todo list with a user (owner or shared)
    // - cannot add duplicate user
    // - owner should not be added to UserIds

    // get user to which a certain todo list is connected

    // remove a certain user from todo list
    // - user can remove themselves
    // - shared users can delete everyone, except the owner

    private readonly ITodoListService _todoListService;

    public TodoListController(ITodoListService todoListService)
    {
        _todoListService = todoListService;
    }

    // GET many (owner or shared)
    [HttpGet("")]
    public async Task<List<TodoListResponse>> GetMany([FromQuery] TodoListFilterRequest filterRequest) // TODO - userId
    {
        TodoListFilter filter = filterRequest.ToFilter();
        OffsetPagination pager = filterRequest.ToPager();

        OffsetCollection<TodoListModel> offsetCollection = await _todoListService.GetMany(filter, pager);
        List<TodoListResponse> response = offsetCollection.Select(_ => _.ToResponse()).ToList();
        return response;
    }

    // GET one (owner or shared)
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetails([FromRoute] int id, [FromQuery] int userId)
    {
        var model = await _todoListService.GetById(id, userId);

        return Ok(model.ToResponse());
    }

    [HttpPost("")]
    public async Task<IActionResult> Create(CreateTodoListRequest request)
    {
        CreateTodoListModel model = request.ToModel();
        TodoListModel todoList = await _todoListService.Create(model);

        return CreatedAtAction(nameof(GetDetails), new { id = todoList.Id, userId = todoList.OwnerId },
            todoList.ToResponse());
    }

    // UPDATE (owner or shared)
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        [FromRoute] int id,
        [FromQuery] int userId,
        [FromBody] UpdateTodoListRequest request)
    {
        UpdateTodoListModel model = request.ToModel(id, userId);

        TodoListModel result = await _todoListService.Update(model);

        return Ok(result.ToResponse());
    }
    
    // DELETE (only owner)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, [FromQuery] int userId)
    {
        var success = await _todoListService.Delete(id, userId);

        if (!success)
        {
            return Forbid();
        }

        return NoContent();
    }
    
    // SHARE with user (owner or shared)
    [HttpPost("{id}/users")]
    public async Task<IActionResult> AddUser(
        [FromRoute] int id,
        [FromQuery] int userId,
        [FromBody] AddUserToTodoListRequest request)
    {
        var success = await _todoListService.AddUser(id, userId, request.UserId);

        if (!success)
        {
            return Forbid();
        }

        return Ok();
    }
    
    // GET users of list (owner or shared)
    [HttpGet("{id}/users")]
    public async Task<IActionResult> GetUsers(
        [FromRoute] int id,
        [FromQuery] int userId)
    {
        var users = await _todoListService.GetUsers(id, userId);

        return Ok(users);
    }

    // REMOVE user (owner or shared)
    [HttpDelete("{id}/users/{targetUserId}")]
    public async Task<IActionResult> RemoveUser(
        [FromRoute] int id,
        [FromRoute] int targetUserId,
        [FromQuery] int userId)
    {
        var success = await _todoListService.RemoveUser(id, userId, targetUserId);

        if (!success)
        {
            return Forbid();
        }

        return NoContent();
    }
}