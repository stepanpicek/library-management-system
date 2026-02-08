using System.Net;
using System.Net.Http.Json;
using LibraryManagementSystem.Web.IntegrationTests.Common;

namespace LibraryManagementSystem.Web.IntegrationTests.Books;

public class ReturnBookTests(TestWebApplicationFactory factory)
    : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client = factory.CreateClient();

    public Task InitializeAsync() => factory.ResetDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ReturnBook_WithOpenLoan_ReturnsAccepted()
    {
        // Arrange - 1984 has open loans
        var books = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=1984");
        var book = books!.Items.First();

        // Act
        var response = await _client.PutAsync($"/api/books/{book.Id}/return", null);

        // Assert
        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    }

    [Fact]
    public async Task ReturnBook_IncreasesAvailableCopies()
    {
        // Arrange - 1984 has 0 available copies but has open loans
        var books = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=1984");
        var bookBefore = books!.Items.First();
        var initialCopies = bookBefore.AvailableCopies;

        // Act
        await _client.PutAsync($"/api/books/{bookBefore.Id}/return", null);

        // Assert
        var booksAfter = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=1984");
        var bookAfter = booksAfter!.Items.First();
        Assert.Equal(initialCopies + 1, bookAfter.AvailableCopies);
    }

    [Fact]
    public async Task ReturnBook_WithNoOpenLoan_ReturnsBadRequest()
    {
        // Arrange - R.U.R. has no open loans (only closed ones)
        var books = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=R.U.R.");
        var book = books!.Items.First();

        // Act
        var response = await _client.PutAsync($"/api/books/{book.Id}/return", null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ReturnBook_WithNonExistentBook_ReturnsBadRequest()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.PutAsync($"/api/books/{nonExistentId}/return", null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ReturnBook_MultipleTimes_IncreasesAvailableCopiesCorrectly()
    {
        // Arrange - 1984 has 2 open loans
        var books = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=1984");
        var book = books!.Items.First();
        var initialCopies = book.AvailableCopies;

        // Act - Return twice
        await _client.PutAsync($"/api/books/{book.Id}/return", null);
        await _client.PutAsync($"/api/books/{book.Id}/return", null);

        // Assert
        var booksAfter = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=1984");
        var bookAfter = booksAfter!.Items.First();
        Assert.Equal(initialCopies + 2, bookAfter.AvailableCopies);
    }

    [Fact]
    public async Task ReturnBook_AllLoans_LastReturnFails()
    {
        // Arrange - 1984 has 2 open loans, return both
        var books = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=1984");
        var book = books!.Items.First();

        // Act - Return all open loans
        var response1 = await _client.PutAsync($"/api/books/{book.Id}/return", null);
        Assert.Equal(HttpStatusCode.Accepted, response1.StatusCode);

        var response2 = await _client.PutAsync($"/api/books/{book.Id}/return", null);
        Assert.Equal(HttpStatusCode.Accepted, response2.StatusCode);

        // Try to return one more (no open loans left)
        var response3 = await _client.PutAsync($"/api/books/{book.Id}/return", null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response3.StatusCode);
    }

    [Fact]
    public async Task BorrowAndReturn_WorksCorrectly()
    {
        // Arrange - R.U.R. has no open loans
        var books = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=R.U.R.");
        var book = books!.Items.First();
        var initialCopies = book.AvailableCopies;

        // Act - Borrow and then return
        var borrowResponse = await _client.PutAsync($"/api/books/{book.Id}/borrow", null);
        Assert.Equal(HttpStatusCode.Accepted, borrowResponse.StatusCode);

        var returnResponse = await _client.PutAsync($"/api/books/{book.Id}/return", null);
        Assert.Equal(HttpStatusCode.Accepted, returnResponse.StatusCode);

        // Assert - Available copies should be back to initial
        var booksAfter = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books?text=R.U.R.");
        var bookAfter = booksAfter!.Items.First();
        Assert.Equal(initialCopies, bookAfter.AvailableCopies);
    }

    [Fact]
    public async Task BorrowAndReturn_MultipleBooks_WorksCorrectly()
    {
        // Arrange
        var allBooks = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books");
        var booksWithCopies = allBooks!.Items.Where(b => b.AvailableCopies > 0).Take(2).ToList();
        Assert.True(booksWithCopies.Count >= 2);

        var book1 = booksWithCopies[0];
        var book2 = booksWithCopies[1];
        var initialCopies1 = book1.AvailableCopies;
        var initialCopies2 = book2.AvailableCopies;

        // Act - Borrow both books
        await _client.PutAsync($"/api/books/{book1.Id}/borrow", null);
        await _client.PutAsync($"/api/books/{book2.Id}/borrow", null);

        // Return both books
        await _client.PutAsync($"/api/books/{book1.Id}/return", null);
        await _client.PutAsync($"/api/books/{book2.Id}/return", null);

        // Assert
        var booksAfter = await _client.GetFromJsonAsync<PaginatedResponse<BookResponse>>("/api/books");
        var book1After = booksAfter!.Items.First(b => b.Id == book1.Id);
        var book2After = booksAfter!.Items.First(b => b.Id == book2.Id);

        Assert.Equal(initialCopies1, book1After.AvailableCopies);
        Assert.Equal(initialCopies2, book2After.AvailableCopies);
    }
}