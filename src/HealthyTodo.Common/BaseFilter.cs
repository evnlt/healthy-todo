using HealthyTodo.Common.Constants;

namespace HealthyTodo.Common;

public abstract class BaseFilter<TOrder>
{
    public TOrder OrderBy { get; set; } = default!;
    public OrderDirection OrderDirection { get; set; }
}