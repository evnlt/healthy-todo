using AutoFixture;
using FluentAssertions;
using HealthyTodo.DAL.Entities;
using HealthyTodo.DAL.Extensions;
using HealthyTodo.Models.TodoList;

namespace HealthyTodo.DAL.Tests.Extensions;

public class TodoListEntityExtensionsTests
{
    private readonly Fixture _fixture;

    public TodoListEntityExtensionsTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void ToModel_CorrectData_Success()
    {
        // Arrange
        TodoListEntity entity = _fixture.Create<TodoListEntity>();

        TodoListModel expected = new TodoListModel
        {
            Id = entity.Id,
            Title = entity.Title,
            CreatedDate = entity.CreatedDate,
            OwnerId = entity.OwnerId,
            SharedUserIds = entity.UserIds
        };

        // Act
        TodoListModel actual = entity.ToModel();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToEntity_CorrectData_Success()
    {
        // Arrange
        CreateTodoListModel model = _fixture.Create<CreateTodoListModel>();

        DateTime before = DateTime.UtcNow;

        // Act
        TodoListEntity actual = model.ToEntity();

        DateTime after = DateTime.UtcNow;

        // Assert
        actual.Title.Should().Be(model.Title);
        actual.OwnerId.Should().Be(model.OwnerId);

        actual.CreatedDate.Should().BeOnOrAfter(before);
        actual.CreatedDate.Should().BeOnOrBefore(after);
    }
}