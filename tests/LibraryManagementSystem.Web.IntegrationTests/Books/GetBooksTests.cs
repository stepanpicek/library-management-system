using System.Net;
using System.Net.Http.Json;
using LibraryManagementSystem.Web.IntegrationTests.Common;

namespace LibraryManagementSystem.Web.IntegrationTests.Books;

public class GetBooksTests(TestWebApplicationFactory factory)
    : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client = factory.CreateClient();

    public Task InitializeAsync() => factory.ResetDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetBooks_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/api/books");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetBooks_ReturnsAllSeededBooks()
    {
        var result = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books");

        Assert.NotNull(result);
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(3, result.Items.Count);
    }

    [Fact]
    public async Task GetBooks_ReturnsBooksSortedByTitle()
    {
        var result = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books");

        Assert.NotNull(result);
        var titles = result.Items.Select(b => b.Title).ToList();
        Assert.Equal(titles.OrderBy(t => t), titles);
    }

    [Theory]
    [InlineData("Malý princ")]
    [InlineData("1984")]
    [InlineData("R.U.R.")]
    public async Task GetBooks_FilterByTitle_ReturnsMatchingBook(string title)
    {
        var result = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>(
            $"/api/books?text={Uri.EscapeDataString(title)}");

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(title, result.Items.First().Title);
    }

    [Theory]
    [InlineData("Antoine")]
    [InlineData("Orwell")]
    [InlineData("Čapek")]
    public async Task GetBooks_FilterByAuthor_ReturnsMatchingBook(string authorPart)
    {
        var result = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>(
            $"/api/books?text={Uri.EscapeDataString(authorPart)}");

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Contains(authorPart, result.Items.First().Author);
    }

    [Theory]
    [InlineData("978-80-00-07359-0", "Malý princ")]
    [InlineData("978-80-7335-647-7", "1984")]
    [InlineData("80-86216-46-2", "R.U.R.")]
    public async Task GetBooks_FilterByIsbn_ReturnsMatchingBook(string isbn, string expectedTitle)
    {
        var result = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>(
            $"/api/books?text={Uri.EscapeDataString(isbn)}");

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(expectedTitle, result.Items.First().Title);
    }

    [Fact]
    public async Task GetBooks_FilterByPartialIsbn_ReturnsMatchingBooks()
    {
        var result = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=978-80");

        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count);
    }

    [Fact]
    public async Task GetBooks_FilterByNonExistingText_ReturnsEmptyList()
    {
        var result = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>(
            "/api/books?text=NonExistingBook");

        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }

    [Fact]
    public async Task GetBooks_WithPagination_ReturnsCorrectPage()
    {
        var result = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?page=1&size=2");

        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(2, result.TotalPages);
        Assert.Equal(1, result.Page);
    }

    [Fact]
    public async Task GetBooks_WithPagination_SecondPage_ReturnsRemainingItems()
    {
        var result = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?page=2&size=2");

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(2, result.Page);
    }

    [Fact]
    public async Task GetBooks_WithPaginationBeyondTotalPages_ReturnsEmptyItems()
    {
        var result = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?page=10&size=2");

        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(3, result.TotalCount);
    }

    [Fact]
    public async Task GetBooks_WithFilterAndPagination_ReturnsFilteredAndPaginatedResults()
    {
        var result = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>(
            "/api/books?text=978-80&page=1&size=1");

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(2, result.TotalPages);
    }

    [Fact]
    public async Task GetBooks_ReturnsCorrectBookProperties()
    {
        var result = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=1984");

        Assert.NotNull(result);
        var book = Assert.Single(result.Items);

        Assert.NotEqual(Guid.Empty, book.Id);
        Assert.Equal("1984", book.Title);
        Assert.Equal("George Orwell", book.Author);
        Assert.Equal(1949, book.Year);
        Assert.Equal("978-80-7335-647-7", book.Isbn);
        Assert.Equal(0, book.AvailableCopies);
    }

    [Fact]
    public async Task GetBooks_WithWhitespaceOnlyFilter_ReturnsAllBooks()
    {
        var result = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=%20%20%20");

        Assert.NotNull(result);
        Assert.Equal(3, result.TotalCount);
    }
}