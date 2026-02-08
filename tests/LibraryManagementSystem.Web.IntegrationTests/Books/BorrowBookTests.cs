using System.Net;
using System.Net.Http.Json;
using LibraryManagementSystem.Web.IntegrationTests.Common;

namespace LibraryManagementSystem.Web.IntegrationTests.Books;

public class BorrowBookTests(TestWebApplicationFactory factory)
    : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client = factory.CreateClient();

    public Task InitializeAsync() => factory.ResetDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task BorrowBook_WithAvailableCopies_ReturnsCreated()
    {
        // Arrange - Get a book with available copies (Malý princ has 9)
        var books = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=Malý princ");
        var book = books!.Items.First();
        Assert.True(book.AvailableCopies > 0);

        // Act
        var response = await _client.PutAsync($"/api/books/{book.Id}/borrow", null);

        // Assert
        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    }

    [Fact]
    public async Task BorrowBook_DecreasesAvailableCopies()
    {
        // Arrange
        var books = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=Malý princ");
        var bookBefore = books!.Items.First();
        var initialCopies = bookBefore.AvailableCopies;

        // Act
        await _client.PutAsync($"/api/books/{bookBefore.Id}/borrow", null);

        // Assert
        var booksAfter = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=Malý princ");
        var bookAfter = booksAfter!.Items.First();
        Assert.Equal(initialCopies - 1, bookAfter.AvailableCopies);
    }

    [Fact]
    public async Task BorrowBook_WithNoAvailableCopies_ReturnsBadRequest()
    {
        // Arrange - 1984 has 0 available copies
        var books = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=1984");
        var book = books!.Items.First();
        Assert.Equal(0, book.AvailableCopies);

        // Act
        var response = await _client.PutAsync($"/api/books/{book.Id}/borrow", null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BorrowBook_WithNonExistentBook_ReturnsBadRequest()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.PutAsync($"/api/books/{nonExistentId}/borrow", null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BorrowBook_MultipleTimes_DecreasesAvailableCopiesCorrectly()
    {
        // Arrange - R.U.R. has 5 available copies
        var books = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=R.U.R.");
        var book = books!.Items.First();
        var initialCopies = book.AvailableCopies;

        // Act - Borrow twice
        await _client.PutAsync($"/api/books/{book.Id}/borrow", null);
        await _client.PutAsync($"/api/books/{book.Id}/borrow", null);

        // Assert
        var booksAfter = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=R.U.R.");
        var bookAfter = booksAfter!.Items.First();
        Assert.Equal(initialCopies - 2, bookAfter.AvailableCopies);
    }

    [Fact]
    public async Task BorrowBook_UntilNoCopiesLeft_LastBorrowFails()
    {
        // Arrange - R.U.R. has 5 available copies, borrow all
        var books = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=R.U.R.");
        var book = books!.Items.First();
        var availableCopies = book.AvailableCopies;

        // Act - Borrow all available copies
        for (var i = 0; i < availableCopies; i++)
        {
            var borrowResponse = await _client.PutAsync($"/api/books/{book.Id}/borrow", null);
            Assert.Equal(HttpStatusCode.Accepted, borrowResponse.StatusCode);
        }

        // Try to borrow one more
        var lastResponse = await _client.PutAsync($"/api/books/{book.Id}/borrow", null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, lastResponse.StatusCode);

        var booksAfter = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=R.U.R.");
        Assert.Equal(0, booksAfter!.Items.First().AvailableCopies);
    }
}