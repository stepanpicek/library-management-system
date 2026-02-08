namespace LibraryManagementSystem.Web.IntegrationTests.Common;

public class PaginatedResponse<T>
{
    public IReadOnlyCollection<T> Items { get; set; } = [];
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
}