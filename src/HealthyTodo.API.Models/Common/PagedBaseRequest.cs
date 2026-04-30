namespace HealthyTodo.API.Models.Common;

public class PagedBaseRequest
{
    public int Offset { get; set; } = 0;
    public int Take { get; set; } = 20;
}