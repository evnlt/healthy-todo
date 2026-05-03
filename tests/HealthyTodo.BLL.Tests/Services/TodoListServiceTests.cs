using AutoFixture;
using FluentAssertions;
using HealthyTodo.BLL.Abstraction.Validators;
using HealthyTodo.BLL.Exceptions;
using HealthyTodo.BLL.Services;
using HealthyTodo.DAL.Abstraction;
using HealthyTodo.Models.TodoList;
using Microsoft.Extensions.Logging;
using Moq;

namespace HealthyTodo.BLL.Tests.Services;

public class TodoListServiceTests
{
    private readonly Fixture _fixture;
    private readonly Mock<ITodoListStore> _storeMock;
    private readonly Mock<ITodoListValidator> _validatorMock;
    private readonly Mock<ILogger<TodoListService>> _loggerMock;

    private readonly TodoListService _sut;

    public TodoListServiceTests()
    {
        _fixture = new Fixture();
        _storeMock = new Mock<ITodoListStore>();
        _validatorMock = new Mock<ITodoListValidator>();
        _loggerMock = new Mock<ILogger<TodoListService>>();

        _sut = new TodoListService(_storeMock.Object, _validatorMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetById_UserHasAccess_ReturnsList()
    {
        // Arrange
        var list = _fixture.Create<TodoListModel>();
        int userId = list.OwnerId;

        _storeMock.Setup(x => x.GetById(list.Id)).ReturnsAsync(list);

        // Act
        var result = await _sut.GetById(list.Id, userId);

        // Assert
        result.Should().Be(list);
    }

    [Fact]
    public async Task GetById_NotFound_ThrowsNotFoundException()
    {
        // Arrange
        string id = _fixture.Create<string>();

        _storeMock.Setup(x => x.GetById(id)).ReturnsAsync((TodoListModel?)null);

        // Act
        var act = async () => await _sut.GetById(id, 1);

        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetById_NoAccess_ThrowsForbiddenException()
    {
        // Arrange
        var list = _fixture.Create<TodoListModel>();
        list.SharedUserIds = new List<int>(); // no access
        int userId = int.MinValue;

        _storeMock.Setup(x => x.GetById(list.Id)).ReturnsAsync(list);

        // Act
        var act = async () => await _sut.GetById(list.Id, userId);

        // Assert
        await act
            .Should()
            .ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task Create_ValidModel_ReturnsCreated()
    {
        // Arrange
        var model = _fixture.Create<CreateTodoListModel>();
        var expected = _fixture.Create<TodoListModel>();

        _storeMock.Setup(x => x.Create(model)).ReturnsAsync(expected);

        // Act
        var result = await _sut.Create(model);

        // Assert
        result.Should().Be(expected);

        _validatorMock.Verify(x => x.Validate(model), Times.Once);
    }

    [Fact]
    public async Task Update_UserHasAccess_Updates()
    {
        // Arrange
        var model = _fixture.Create<UpdateTodoListModel>();
        var existing = _fixture.Create<TodoListModel>();
        existing.Id = model.Id;
        existing.OwnerId = model.UserId;

        var updated = _fixture.Create<TodoListModel>();

        _storeMock.Setup(x => x.GetById(model.Id)).ReturnsAsync(existing);
        _storeMock.Setup(x => x.Update(model)).ReturnsAsync(updated);

        // Act
        var result = await _sut.Update(model);

        // Assert
        result.Should().Be(updated);
    }

    [Fact]
    public async Task Update_NotFound_ThrowsNotFoundException()
    {
        // Arrange
        var model = _fixture.Create<UpdateTodoListModel>();

        _storeMock.Setup(x => x.GetById(model.Id)).ReturnsAsync((TodoListModel?)null);

        // Act
        var act = async () => await _sut.Update(model);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Update_NoAccess_ThrowsForbiddenException()
    {
        // Arrange
        var model = _fixture.Create<UpdateTodoListModel>();
        var existing = _fixture.Create<TodoListModel>();

        existing.Id = model.Id;
        existing.SharedUserIds = []; // no access
        existing.OwnerId = int.MaxValue;

        _storeMock.Setup(x => x.GetById(model.Id)).ReturnsAsync(existing);

        // Act
        var act = async () => await _sut.Update(model);

        await act
            .Should()
            .ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task Delete_UserIsOwner_Success()
    {
        // Arrange
        var list = _fixture.Create<TodoListModel>();
        int userId = list.OwnerId;

        _storeMock.Setup(x => x.GetById(list.Id)).ReturnsAsync(list);

        // Act
        var result = await _sut.Delete(list.Id, userId);

        // Assert
        result.Should().BeTrue();

        _storeMock.Verify(x => x.Delete(list.Id), Times.Once);
    }

    [Fact]
    public async Task Delete_UserIsNotOwner_ThrowsForbiddenException()
    {
        // Arrange
        var list = _fixture.Create<TodoListModel>();

        _storeMock.Setup(x => x.GetById(list.Id)).ReturnsAsync(list);

        // Act
        var act = async () => await _sut.Delete(list.Id, -1);

        // Assert
        await act
            .Should()
            .ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task AddUser_ValidData_Success()
    {
        // Arrange
        var list = _fixture.Create<TodoListModel>();
        list.SharedUserIds = [];
        int userId = list.OwnerId;
        int targetUserId = _fixture.Create<int>();

        _storeMock.Setup(x => x.GetById(list.Id)).ReturnsAsync(list);

        // Act
        var result = await _sut.AddUser(list.Id, userId, targetUserId);

        // Assert
        result.Should().BeTrue();
        list.SharedUserIds.Should().Contain(targetUserId);

        _storeMock.Verify(x => x.UpdateUsers(list.Id, list.SharedUserIds), Times.Once);
    }

    [Fact]
    public async Task AddUser_AlreadyExists_ThrowsBadRequestException()
    {
        // Arrange
        var list = _fixture.Create<TodoListModel>();
        int userId = list.OwnerId;
        int targetUserId = _fixture.Create<int>();

        list.SharedUserIds = [targetUserId];

        _storeMock.Setup(x => x.GetById(list.Id)).ReturnsAsync(list);

        // Act
        var act = async () => await _sut.AddUser(list.Id, userId, targetUserId);

        // Assert
        await act
            .Should()
            .ThrowAsync<BadRequestException>();
    }

    [Fact]
    public async Task RemoveUser_Valid_Removes()
    {
        // Arrange
        var list = _fixture.Create<TodoListModel>();
        int userId = list.OwnerId;
        int targetUserId = _fixture.Create<int>();

        list.SharedUserIds = [targetUserId];

        _storeMock.Setup(x => x.GetById(list.Id)).ReturnsAsync(list);

        // Act
        var result = await _sut.RemoveUser(list.Id, userId, targetUserId);

        // Assert
        result.Should().BeTrue();
        list.SharedUserIds.Should().NotContain(targetUserId);
    }
}