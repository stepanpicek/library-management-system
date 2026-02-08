namespace LibraryManagementSystem.Application.Books.Queries.GetBooks;

public class BookDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public int Year { get; set; }
    public required string Isbn { get; set; }
    public int AvailableCopies { get; set; }
}