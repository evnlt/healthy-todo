using HealthyTodo.BLL.Abstraction.Validators;
using HealthyTodo.BLL.Exceptions;
using HealthyTodo.Common;
using HealthyTodo.Models.TodoList;

namespace HealthyTodo.BLL.Validators;

internal class TodoListValidator : ITodoListValidator
{
    private const int MaxPageSize = 100;
    private const int MaxTodoListTitleLength = 255; // TODO - put in a db constant 
    
    public Task Validate(TodoListFilter filter, OffsetPagination pager)
    {
        ArgumentNullException.ThrowIfNull(filter);
        ArgumentNullException.ThrowIfNull(pager);

        if (filter.UserId <= 0)
        {
            throw new BadRequestException("UserId must be greater than 0");
        }

        if (filter.Ids.Length != 0)
        {
            if (filter.Ids.Any(string.IsNullOrEmpty))
            {
                throw new BadRequestException("Ids must not be null or empty");
            }

            if (filter.Ids.Length != filter.Ids.Distinct().Count())
            {
                throw new BadRequestException("Ids cannot contain duplicates");
            }
        }

        if (pager.Offset < 0)
        {
            throw new BadRequestException("Offset cannot be negative");
        }

        if (pager.Take <= 0)
        {
            throw new BadRequestException("Take must be greater than 0");
        }

        if (pager.Take > MaxPageSize)
        {
            throw new BadRequestException($"Take cannot exceed {MaxPageSize}");
        }

        return Task.CompletedTask;
    }

    public Task Validate(CreateTodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (string.IsNullOrWhiteSpace(model.Title))
        {
            throw new BadRequestException("Title is required");
        }

        if (model.Title.Length > MaxTodoListTitleLength)
        {
            throw new BadRequestException("Title cannot exceed 255 characters");
        }

        if (model.OwnerId <= 0)
        {
            throw new BadRequestException("OwnerId must be greater than 0");
        }

        return Task.CompletedTask;
    }

    public Task Validate(UpdateTodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (string.IsNullOrEmpty(model.Id))
        {
            throw new BadRequestException("Id must not be null or empty");
        }

        if (string.IsNullOrWhiteSpace(model.Title))
        {
            throw new BadRequestException("Title is required");
        }

        if (model.Title.Length > MaxTodoListTitleLength)
        {
            throw new BadRequestException("Title cannot exceed 255 characters");
        }

        if (model.UserId <= 0) 
        {
            throw new BadRequestException("UserId must be greater than 0");
        }

        return Task.CompletedTask;
    }
}