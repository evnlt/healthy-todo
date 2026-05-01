namespace HealthyTodo.BLL.Exceptions;

public static class ErrorMessages
{
    public const string TodoListNotFound = "Todo list not found";
    public const string AccessDenied = "Access denied";
    public const string OnlyOwnerCanDelete = "Only owner can delete";

    public const string CannotAddOwner = "Cannot add owner";
    public const string UserAlreadyAdded = "User already added";

    public const string CannotRemoveOwner = "Cannot remove owner";
    public const string UserNotInList = "User not in list";
}