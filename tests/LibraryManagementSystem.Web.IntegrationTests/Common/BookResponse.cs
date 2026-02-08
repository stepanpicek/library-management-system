namespace LibraryManagementSystem.Web.IntegrationTests.Common;

public class BookResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Isbn { get; set; } = string.Empty;
    public int AvailableCopies { get; set; }
}