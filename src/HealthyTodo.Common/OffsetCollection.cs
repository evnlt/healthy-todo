using System.Collections.ObjectModel;

namespace HealthyTodo.Common;

public class OffsetCollection<T>(IList<T> list, int offset, int totalCount) : ReadOnlyCollection<T>(list)
{
    public int Offset { get; } = offset;

    public int TotalCount { get; } = totalCount;
}