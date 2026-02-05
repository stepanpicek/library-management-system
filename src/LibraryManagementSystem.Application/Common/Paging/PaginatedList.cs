using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Application.Common.Paging;

public class PaginatedList<T>(IReadOnlyCollection<T> items, int count, int page, int pageSize)
{
    public IReadOnlyCollection<T> Items => items;
    public int Page => page;
    public int TotalPages => (int)Math.Ceiling(count / (double)pageSize);
    public int TotalCount => count;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int? page, int? size,
        CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);
        if (page is null || size is null)
        {
            return new PaginatedList<T>(source.ToList(), count, 1, count);
        }

        var pageNumber = page.Value;
        var pageSize = size.Value;
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}