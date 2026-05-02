using AutoFixture;
using FluentAssertions;
using HealthyTodo.API.Extensions;
using HealthyTodo.API.Models.TodoList;
using HealthyTodo.Models.TodoList;

namespace HealthyTodo.API.Tests.Extensions;

public class TodoListExtensionsTests
{
    private readonly Fixture _fixture;

    public TodoListExtensionsTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void ToResponse_CorrectData_Success()
    {
        // Arrange
        TodoListModel model = _fixture.Create<TodoListModel>();

        TodoListResponse expected = new TodoListResponse
        {
            Id = model.Id,
            Title = model.Title,
            CreatedDate = model.CreatedDate,
            OwnerId = model.OwnerId
        };

        // Act
        TodoListResponse actual = model.ToResponse();

        // Assert
        actual.Should()
            .BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ToCreateModel_CorrectData_Success()
    {
        // Arrange
        CreateTodoListRequest request = _fixture.Create<CreateTodoListRequest>();

        CreateTodoListModel expected = new CreateTodoListModel
        {
            Title = request.Title,
            OwnerId = request.OwnerId
        };

        // Act
        CreateTodoListModel actual = request.ToModel();

        // Assert
        actual.Should()
            .BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ToUpdateModel_CorrectData_Success()
    {
        // Arrange
        UpdateTodoListRequest request = _fixture.Create<UpdateTodoListRequest>();
        string listId = _fixture.Create<string>();
        int userId = _fixture.Create<int>();

        UpdateTodoListModel expected = new UpdateTodoListModel
        {
            Id = listId,
            UserId = userId,
            Title = request.Title,
        };

        // Act
        UpdateTodoListModel actual = request.ToModel(listId, userId);

        // Assert
        actual.Should()
            .BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ToFilter_CorrectData_Success()
    {
        // Arrange
        TodoListFilterRequest filterRequest = _fixture.Create<TodoListFilterRequest>();

        TodoListFilter expected = new TodoListFilter
        {
            Ids = filterRequest.Ids,
            UserId = filterRequest.UserId,
            OrderBy = filterRequest.OrderBy,
            OrderDirection = filterRequest.OrderDirection
        };

        // Act
        TodoListFilter actual = filterRequest.ToFilter();

        // Assert
        actual.Should()
            .BeEquivalentTo(expected);
    }
}