using HealthyTodo.Common;

namespace HealthyTodo.API.Models.Common.Extensions;

public static class BaseFilterRequestExtensions
{
    public static OffsetPagination ToPager(this PagedBaseRequest request)
    {
        return new OffsetPagination(request.Offset, request.Take);
    }
}