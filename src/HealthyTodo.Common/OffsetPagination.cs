namespace HealthyTodo.Common;

public class OffsetPagination
{
    public OffsetPagination(int offset, int take)
    {
        Offset = offset;
        Take = take;
    }

    public int Offset { get; }

    public int Take { get; }

    public static OffsetPagination Default { get; } = new OffsetPagination(0, 100);

    public static OffsetPagination First { get; } = new OffsetPagination(0, 1);

    public static OffsetPagination None { get; } = new OffsetPagination(0, int.MaxValue);
}