using LibraryManagementSystem.Application.Common.Paging;

namespace LibraryManagementSystem.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static Task<PaginatedList<T>> Paginate<T>(this IQueryable<T> queryable, int? page, int? size) =>
        PaginatedList<T>.CreateAsync(queryable, page, size);
}