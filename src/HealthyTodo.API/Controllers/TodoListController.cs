using System.ComponentModel.DataAnnotations;
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
    private readonly ITodoListService _todoListService;

    public TodoListController(ITodoListService todoListService)
    {
        _todoListService = todoListService;
    }

    // GET many (owner or shared)
    [HttpGet("")]
    public async Task<List<TodoListResponse>> GetMany([FromQuery] TodoListFilterRequest filterRequest)
    {
        TodoListFilter filter = filterRequest.ToFilter();
        OffsetPagination pager = filterRequest.ToPager();

        OffsetCollection<TodoListModel> offsetCollection = await _todoListService.GetMany(filter, pager);
        List<TodoListResponse> response = offsetCollection.Select(_ => _.ToResponse()).ToList();
        return response;
    }

    // GET one (owner or shared)
    [HttpGet("{listId}")]
    public async Task<IActionResult> GetDetails([FromRoute] string listId, [FromQuery, Required] int userId)
    {
        var model = await _todoListService.GetById(listId, userId);

        return Ok(model.ToResponse());
    }

    [HttpPost("")]
    public async Task<IActionResult> Create(CreateTodoListRequest request)
    {
        CreateTodoListModel model = request.ToModel();
        TodoListModel todoList = await _todoListService.Create(model);

        return CreatedAtAction(nameof(GetDetails), new { listId = todoList.Id, userId = todoList.OwnerId },
            todoList.ToResponse());
    }

    // UPDATE (owner or shared)
    [HttpPut("{listId}")]
    public async Task<IActionResult> Update(
        [FromRoute] string listId,
        [FromQuery, Required] int userId,
        [FromBody] UpdateTodoListRequest request)
    {
        UpdateTodoListModel model = request.ToModel(listId, userId);

        TodoListModel result = await _todoListService.Update(model);

        return Ok(result.ToResponse());
    }
    
    // DELETE (only owner)
    [HttpDelete("{listId}")]
    public async Task<IActionResult> Delete([FromRoute] string listId, [FromQuery, Required] int userId)
    {
        var success = await _todoListService.Delete(listId, userId);

        if (!success)
        {
            return Forbid();
        }

        return NoContent();
    }
    
    // SHARE with user (owner or shared)
    [HttpPost("{listId}/users")]
    public async Task<IActionResult> AddUser(
        [FromRoute] string listId,
        [FromQuery, Required] int userId,
        [FromBody] AddUserToTodoListRequest request)
    {
        var success = await _todoListService.AddUser(listId, userId, request.UserId);

        if (!success)
        {
            return Forbid();
        }

        return Ok();
    }
    
    // GET users of list (owner or shared)
    [HttpGet("{listId}/users")]
    public async Task<IActionResult> GetUsers(
        [FromRoute] string listId,
        [FromQuery, Required] int userId)
    {
        var users = await _todoListService.GetUsers(listId, userId);

        return Ok(users);
    }

    // REMOVE user (owner or shared)
    [HttpDelete("{listId}/users/{targetUserId}")]
    public async Task<IActionResult> RemoveUser(
        [FromRoute] string listId,
        [FromRoute] int targetUserId,
        [FromQuery, Required] int userId)
    {
        var success = await _todoListService.RemoveUser(listId, userId, targetUserId);

        if (!success)
        {
            return Forbid();
        }

        return NoContent();
    }
}