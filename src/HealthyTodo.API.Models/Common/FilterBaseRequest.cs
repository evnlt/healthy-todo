using HealthyTodo.Common.Constants;

namespace HealthyTodo.API.Models.Common;

public class FilterBaseRequest<TOrder> : PagedBaseRequest
{
    public TOrder OrderBy { get; set; }
    public OrderDirection OrderDirection { get; set; }
}